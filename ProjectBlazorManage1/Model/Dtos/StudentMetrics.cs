namespace ProjectBlazorManage1.Model.Dtos
{
    public class StudentMetrics
    {
        public double Book1Score { get; set; }
        public double Book2Score { get; set; }
        public double Book3Score { get; set; }

        // คะแนนกิจกรรมพิเศษ
        public double OrientationScore { get; set; }             // ปฐมนิเทศ
        public double Activity1Score { get; set; }               // กิจกรรมสัมพันธ์ 1
        public double Activity2Score { get; set; }               // กิจกรรมสัมพันธ์ 2
        public double Activity3AndClosingScore { get; set; }     // กิจกรรมสัมพันธ์ 3 + ปัจฉิมนิเทศ
        public double FeedbackScore { get; set; }                // แสดงความรู้สึก
        public double TestScore { get; set; }                    // สอบ

        // คะแนนอื่นๆ
        public double DifferentBranchScore { get; set; }         // เรียนต่างสาขา
        public double ExtraClassScore { get; set; }              // เรียนเสริม
        public double ExtraClassScore2 { get; set; }             // เรียนเสริมพิเศษ

        // คะแนนรวม
        public double ExamScore { get; set; }
        // คะแนนรวมทั้งหมดก่อนคูณชั่วโมง
        public double TotalHourScore { get; set; }               // คะแนนรวมหลังคูณชั่วโมง (x2)
        public double TotalScore { get; set; }                   // คะแนนรวมทั้งหมด + คะแนนอื่นๆ

        // สำหรับคิดใบขยัน
        public double TotalScoreS { get; set; }

        // เงื่อนไขผ่าน/ไม่ผ่าน
        public bool ExamEligibility { get; set; }             // สิทธิสอบ
        public bool DiligenceCertificate { get; set; }        // ได้รับใบขยัน
    }
}
