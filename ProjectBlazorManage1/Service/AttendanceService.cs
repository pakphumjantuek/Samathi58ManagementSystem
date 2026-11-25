using ClosedXML.Excel;
using ProjectBlazorManage1.Model.Dtos;

namespace ProjectBlazorManage1.Service
{
    public class AttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LearningPlan>> GetClassDetailsAsync(int classId)
        {      
            var learningPlans = await _context.LearningPlans
                .Include(p => p.teacher)
                .Where(l => l.ClassStudiedId == classId)
                .OrderBy(l => l.Date)
                .ToListAsync();

            return (learningPlans);
        }

        public async Task<AttendanceViewModel?> LoadAttendanceViewModelAsync(int classId, int learningPlanId)
        {
            var learningPlan = await _context.LearningPlans
                .Include(l => l.ClassStudied)
                .FirstOrDefaultAsync(l => l.Id == learningPlanId && l.ClassStudiedId == classId);

            if (learningPlan == null)
            {
                return null;
            }

            var students = await _context.students
                .Where(s => s.ClassStudie_Id == classId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            var existingAttendance = await _context.Attendances
                .Where(a => a.LearningPlanId == learningPlanId)
                .ToDictionaryAsync(a => a.StudentId, a => a);

            return new AttendanceViewModel
            {
                ClassId = classId,
                LearningPlanId = learningPlanId,
                LearningPlanName = learningPlan.Name,
                ClassNumber = learningPlan.ClassStudied.Id,
                Date = learningPlan.Date ?? DateTime.Now,
                Students = students,
                ExistingAttendance = existingAttendance,
                AttendanceRecords = students.Select(s => new Attendance
                {
                    StudentId = s.Id,
                    StudiedType = existingAttendance.ContainsKey(s.Id) ? existingAttendance[s.Id].StudiedType : StudiedType.ขาดเรียน,
                    Remarks = existingAttendance.ContainsKey(s.Id) ? existingAttendance[s.Id].Remarks : "",
                    Extraclass = existingAttendance.ContainsKey(s.Id) ? existingAttendance[s.Id].Extraclass : false
                }).ToList()
            };
        }

        public async Task SaveAttendanceAsync(AttendanceViewModel attendanceViewModel)
        {
            foreach (var attendance in attendanceViewModel.AttendanceRecords)
            {
                var existingRecord = await _context.Attendances
                    .FirstOrDefaultAsync(a =>
                        a.StudentId == attendance.StudentId &&
                        a.LearningPlanId == attendanceViewModel.LearningPlanId);

                if (existingRecord != null)
                {
                    existingRecord.StudiedType = attendance.StudiedType;
                    existingRecord.Remarks = attendance.Remarks;
                    existingRecord.Extraclass = attendance.Extraclass;
                    _context.Update(existingRecord);
                }
                else
                {
                    _context.Attendances.Add(new Attendance
                    {
                        StudentId = attendance.StudentId,
                        LearningPlanId = attendanceViewModel.LearningPlanId,
                        CheckInTime = DateTime.Now,
                        StudiedType = attendance.StudiedType,
                        Remarks = attendance.Remarks,
                        Extraclass = attendance.Extraclass
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AttendanceReportViewModel?> GetAttendanceReportAsync(int classId)
        {
            var classInfo = await _context.ClassStudieds.FirstOrDefaultAsync(c => c.Id == classId);
            if (classInfo == null)
                return null;

            var learningPlans = await _context.LearningPlans
                .Where(lp => lp.ClassStudiedId == classId)
                .OrderBy(lp => lp.Date)
                .ToListAsync();

            var students = await _context.students
                .Where(s => s.ClassStudie_Id == classId)
                .ToListAsync();

            var attendance = await _context.Attendances
                .Where(a => a.LearningPlan.ClassStudiedId == classId)
                .ToListAsync();

            var studentScores = students.ToDictionary(s => s.Id, s =>
                attendance
                    .Where(a => a.StudentId == s.Id &&
                        (a.StudiedType == StudiedType.มาเรียน ||
                         a.StudiedType == StudiedType.เรียนเสริม ||
                         a.StudiedType == StudiedType.เรียนต่างสาขา))
                    .Sum(a => learningPlans.FirstOrDefault(lp => lp.Id == a.LearningPlanId)?.Score ?? 0)
            );

            return new AttendanceReportViewModel
            {
                ClassId = classId,
                ClassNumber = classInfo.Id,
                Students = students,
                LearningPlans = learningPlans,
                AttendanceRecords = attendance,
                StudentScores = studentScores
            };
        }

        public async Task<byte[]> GenerateAttendanceReportExcel(int classId)
        {
            var report = await GetAttendanceReportAsync(classId);

            if (report == null) return Array.Empty<byte>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("รายงานการเข้าเรียน");

                int headerRow = 1;
                int col = 1;

                // หัวตารางหลัก
                worksheet.Cell(headerRow, col++).Value = "ลำดับ";
                worksheet.Cell(headerRow, col++).Value = "รหัสนักเรียน";
                worksheet.Cell(headerRow, col++).Value = "ชื่อ-นามสกุล";

                // หัวข้อวิชา
                foreach (var plan in report.LearningPlans)
                {
                    worksheet.Cell(headerRow, col++).Value = plan.Name;
                }

                worksheet.Cell(headerRow, col).Value = "คะแนนรวม";
                worksheet.Row(headerRow).Style.Font.Bold = true;

                // เติมข้อมูลนักเรียน
                int row = 2;
                for (int i = 0; i < report.Students.Count; i++)
                {
                    var student = report.Students[i];
                    col = 1;

                    worksheet.Cell(row, col++).Value = i + 1;
                    worksheet.Cell(row, col++).Value = student.Id;
                    worksheet.Cell(row, col++).Value = $"{student.FirstName} {student.LastName}";

                    // สถานะการเข้าเรียนของแต่ละวิชา
                    foreach (var plan in report.LearningPlans)
                    {
                        var status = report.GetAttendanceStatus(student.Id, plan.Id);
                        worksheet.Cell(row, col++).Value = status.Type switch
                        {
                            "present" => "มาเรียน",
                            "extra" => "เรียนเสริม",
                            "branch" => "เรียนต่างสาขา",
                            _ => "ขาดเรียน"
                        };
                    }

                    // คะแนนรวม
                    var totalScore = report.StudentScores.ContainsKey(student.Id) ? report.StudentScores[student.Id] : 0;
                    worksheet.Cell(row, col).Value = totalScore;

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }


    }
}
