
namespace ProjectBlazorManage1.Model
{
    public class EducationHistory
    {
        [Key]
        public int EducationHistoryId { get; set; }

        [DisplayName("ระดับการศึกษา")]
        [Required(ErrorMessage = "กรุณาเลือกระดับการศึกษา")]
        public DegreeTypes Degree { get; set; } // เปลี่ยนเป็น Enum
        [DisplayName("จบจากสถาบัน")]
        [Required(ErrorMessage = "กรุณากรอกชื่อสถาบัน")]
        public string Institution { get; set; } // สถาบันที่จบ
        [DisplayName("สาขาเอก")]
        [Required(ErrorMessage = "กรุณากรอกชื่อสาขาเอก")]
        public string MajorName { get; set; } // สถาบันที่จบ
        [DisplayName("ประเทศที่จบ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อประเทศที่จบ")]
        public string YearGraduated { get; set; } // ปีที่จบ
        [DisplayName("วุฒิที่จบ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อวุฒิที่จบ")]
        public DegreeName degreeName { get; set; } // ปีที่จบ

        // Foreign Key เชื่อมไปยัง Teacher
        public int TeacherId { get; set; }
        [ValidateNever]
        public virtual Teacher Teacher { get; set; }
    }
}
