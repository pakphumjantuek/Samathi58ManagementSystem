using ClosedXML.Excel;

namespace ProjectBlazorManage1.Service
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClassReportResult> GetClassReportAsync(int classId)
        {
            var result = new ClassReportResult();

            result.ClassInfo = await _context.ClassStudieds.FindAsync(classId);
            if (result.ClassInfo == null) return result;

            result.LearningPlans = await _context.LearningPlans
                .Where(l => l.ClassStudiedId == classId)
                .OrderBy(l => l.Date)
                .ToListAsync();

            result.Students = await _context.students
                .Where(s => s.ClassStudie_Id == classId)
                .OrderBy(s => s.FirstName)
                .ToListAsync();

            result.AttendanceRecords = await _context.Attendances
                .Include(a => a.LearningPlan)
                .Include(a => a.Students)
                .Where(a => a.LearningPlan.ClassStudiedId == classId)
                .ToListAsync();

            foreach (var student in result.Students)
            {
                var metrics = CalculateMetrics(student, result.AttendanceRecords);
                result.StudentMetricsDict[student.Id] = metrics;
                student.Attendance_Score = metrics.TotalScore;
            }

            await _context.SaveChangesAsync();
            return result;
        }

        private StudentMetrics CalculateMetrics(Students student, List<Attendance> attendanceRecords)
        {
            var attendance = attendanceRecords.Where(a => a.StudentId == student.Id).ToList();
            var metrics = new StudentMetrics();

            metrics.Book1Score = SumScore(attendance, "1");
            metrics.Book2Score = SumScore(attendance, "2");
            metrics.Book3Score = SumScore(attendance, "3");

            metrics.OrientationScore = SumByName(attendance, "ปฐมนิเทศ");
            metrics.Activity1Score = SumByName(attendance, "กิจกรรมสัมพันธ์ 1");
            metrics.Activity2Score = SumByName(attendance, "กิจกรรมสัมพันธ์ 2");
            metrics.Activity3AndClosingScore = SumByName(attendance, "กิจกรรมสัมพันธ์ 3") + SumByName(attendance, "ปัจฉิมนิเทศ");
            metrics.FeedbackScore = SumByName(attendance, "แสดงความรู้สึก");
            metrics.TestScore = SumByName(attendance, "สอบ");

            metrics.DifferentBranchScore = attendance
                .Where(a => a.StudiedType == StudiedType.เรียนต่างสาขา)
                .Sum(a => a.LearningPlan.Score ?? 0);

            metrics.ExtraClassScore = attendance
                .Where(a => a.StudiedType == StudiedType.เรียนเสริม)
                .Sum(a => a.LearningPlan.Score ?? 0);

            metrics.ExtraClassScore2 = attendance
                .Where(a => a.Extraclass == true)
                .Sum(a => a.LearningPlan.Score ?? 0);

            metrics.ExamScore = metrics.Book1Score + metrics.Book2Score + metrics.Book3Score +
                                metrics.DifferentBranchScore + metrics.ExtraClassScore +
                                metrics.OrientationScore + metrics.Activity1Score +
                                metrics.Activity2Score + metrics.Activity3AndClosingScore +
                                metrics.FeedbackScore + metrics.TestScore + metrics.ExtraClassScore2 +
                                (student.credit ?? 0);

            metrics.TotalHourScore = metrics.ExamScore * 2;
            metrics.TotalScore = metrics.TotalHourScore + student.Total;
            metrics.TotalScoreS = metrics.Book1Score + metrics.Book2Score + metrics.Book3Score + metrics.DifferentBranchScore;

            metrics.ExamEligibility = metrics.TotalScore >= 200;
            metrics.DiligenceCertificate = metrics.TotalScoreS >= 96;

            return metrics;
        }

        private int SumScore(List<Attendance> attendance, string bookNumber) =>
            (int)attendance
                .Where(a => a.StudiedType == StudiedType.มาเรียน && a.LearningPlan.Book == bookNumber)
                .Sum(a => a.LearningPlan.Score ?? 0);

        private int SumByName(List<Attendance> attendance, string keyword) =>
            (int)attendance
                .Where(a => (a.StudiedType == StudiedType.มาเรียน || a.StudiedType == StudiedType.เรียนเสริม) &&
                            a.LearningPlan.Name.Contains(keyword))
                .Sum(a => a.LearningPlan.Score ?? 0);

        public async Task<byte[]> GenerateClassReportExcel(int classId)
        {
            var report = await GetClassReportAsync(classId);
            if (report.ClassInfo == null) return Array.Empty<byte>();

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add($"รายงานชั้นเรียน {report.ClassInfo.Id}");

            int row = 1;
            int col = 1;

            string[] headers = {
        "ที่", "รหัสประจำตัว", "ชื่อ-นามสกุล", "ชื่อภาษาอังกฤษ",
        "เล่ม 1", "เล่ม 2", "เล่ม 3",
        "ปฐมนิเทศ", "กิจกรรมสัมพันธ์ 1", "กิจกรรมสัมพันธ์ 2", "กิจกรรมสัมพันธ์ 3 และปัจฉิมนิเทศ",
        "แสดงความรู้สึก", "สอบ",
        "เรียนต่างสาขา", "เรียนเสริม", "เรียนเสริมพิเศษ", "โอนหน่วยกิต",
        "คะแนนรวม", "ชั่วโมงรวม", "การบ้าน","ชั่วโมงทั้งหมด",
        "สิทธิสอบ", "ใบขยัน"
    };

            foreach (var header in headers)
            {
                sheet.Cell(row, col++).Value = header;
            }

            sheet.Row(1).Style.Font.Bold = true;
            sheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

            // รายการนักเรียน
            int index = 1;
            foreach (var student in report.Students)
            {
                var metrics = report.StudentMetricsDict[student.Id];
                col = 1;
                row++;

                sheet.Cell(row, col++).Value = index++;
                sheet.Cell(row, col++).Value = student.PersonalId;
                sheet.Cell(row, col++).Value = $"{student.FirstName}-{student.LastName}";
                sheet.Cell(row, col++).Value = student.EnglishName;

                sheet.Cell(row, col++).Value = metrics.Book1Score;
                sheet.Cell(row, col++).Value = metrics.Book2Score;
                sheet.Cell(row, col++).Value = metrics.Book3Score;

                sheet.Cell(row, col++).Value = metrics.OrientationScore;
                sheet.Cell(row, col++).Value = metrics.Activity1Score;
                sheet.Cell(row, col++).Value = metrics.Activity2Score;
                sheet.Cell(row, col++).Value = metrics.Activity3AndClosingScore;

                sheet.Cell(row, col++).Value = metrics.FeedbackScore;
                sheet.Cell(row, col++).Value = metrics.TestScore;

                sheet.Cell(row, col++).Value = metrics.DifferentBranchScore;
                sheet.Cell(row, col++).Value = metrics.ExtraClassScore;
                sheet.Cell(row, col++).Value = metrics.ExtraClassScore2;
                sheet.Cell(row, col++).Value = student.credit ?? 0;

                sheet.Cell(row, col++).Value = metrics.ExamScore;
                sheet.Cell(row, col++).Value = metrics.TotalHourScore;
                sheet.Cell(row, col++).Value = student.Total;
                sheet.Cell(row, col++).Value = metrics.TotalScore;
                sheet.Cell(row, col++).Value = metrics.ExamEligibility ? "มีสิทธิ์" : "ไม่มีสิทธิ์";
                sheet.Cell(row, col++).Value = metrics.DiligenceCertificate ? "ได้รับ" : "ไม่ได้รับ";
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<List<ClassPassSummary>> GetClassPassSummaryAsync()
        {
            var summaries = new List<ClassPassSummary>();

            // ดึงข้อมูลทุก ClassStudied
            var classes = await _context.ClassStudieds.ToListAsync();

            foreach (var classInfo in classes)
            {
                var report = await GetClassReportAsync(classInfo.Id);
                if (report.ClassInfo == null) continue;

                int totalStudents = report.Students.Count;
                int passedStudents = report.Students.Count(s => report.StudentMetricsDict[s.Id].ExamEligibility);
                int passedStudentsWithCertificate = report.Students.Count(s => report.StudentMetricsDict[s.Id].DiligenceCertificate);

                summaries.Add(new ClassPassSummary
                {
                    ClassId = classInfo.Id,
                    ClassName = classInfo.ClassName, // สมมติว่ามี property Name ใน ClassStudied
                    TotalStudents = totalStudents,
                    PassedStudents = passedStudents,
                    PassPercentage = totalStudents > 0 ? (double)passedStudents / totalStudents * 100 : 0,
                    PassedStudentsWithCertificate = passedStudentsWithCertificate
                });
            }

            return summaries;
        }

    }
}
