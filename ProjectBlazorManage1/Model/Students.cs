using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace ProjectBlazorManage1.Model
{
    public class Students
    {
        [DisplayName("ลำดับที่")]  
        public int Id { get; set; } // ลำดับที่
        [DisplayName("รหัสประจำตัว")]
        public string? PersonalId
        {
            get
            {
                if (ClassStudie_Id.HasValue)
                    return $"{ClassStudie_Id}-58-{ClassStudie_RunningId.ToString("D3")}";
                else
                    return null;
            }
        }
        public int ClassStudie_RunningId { get; set; }

        [DisplayName("รหัสนักศึกษา")]
        public string? Student_Id { get; set; } // รหัสนักเรียน

        [DisplayName("คำนำหน้า")]
        public int Initial_Id { get; set; } // คำนำหน้า
        [ForeignKey("Initial_Id")]
        [ValidateNever]
        public Initial Initial { get; set; } // คำนำหน้า
        [DisplayName("ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อ")]
        public string FirstName { get; set; } // ชื่อ
        [DisplayName("นามสกุล")]
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        public string LastName { get; set; } // สกุล
        [DisplayName("ชื่อเล่น")]
        [Required(ErrorMessage = "กรุณากรอกชื่อเล่น")]
        public string Nickname { get; set; } // ชื่อเล่น
        [DisplayName("อายุ")]
        [Required(ErrorMessage = "กรุณากรอกอายุ")]
        public int? Age
        {
            get
            {
                if (BirthDate == null)
                    return null;

                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "กรูณากรอกเบอร์โทร์ 10 หลัก")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "เบอร์โทรศัพท์ต้องขึ้นต้นด้วย 0")]
        public string Phone_Number { get; set; }
        [DisplayName("ชื่อภาษาอังกฤษ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อภาษาอังกฤษ")]
        public string EnglishName { get; set; } // ชื่อภาษาอังกฤษ
        [DisplayName("วันเกิด")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "กรุณากรอกวันเกิด")]
        public DateTime? BirthDate { get; set; } = DateTime.Today;// วันเกิด
        [DisplayName("วุฒิการศึกษา")]
        public DegreeType degreeType { get; set; } // วุฒิการศึกษา
        [DisplayName("ความสัมพันธ์")]
        public Utility.SD.RelationshipType relationshipType { get; set; } // วุฒิการศึกษา
        [DisplayName("สถาบันการศึกษา")]
        [Required(ErrorMessage = "กรุณากรอกสถาบันการศึกษา")]
        public string Institution { get; set; } // สถาบันการศึกษา

        // ที่อยู่ตามบัตรประจำตัวประชาชน
        [DisplayName("ที่อยู่ตามบัตรประจำตัวประชาชน")]
        [Required(ErrorMessage = "กรุณากรอกที่อยู่ตามบัตรประจำตัวประชาชน")]
        public string CitizenCardAddress { get; set; }
        // ที่อยู่ที่ติดต่อได้
        [DisplayName("ที่อยู่ที่ติดต่อได้")]
        [Required(ErrorMessage = "กรุณากรอกที่อยู่ที่ติดต่อได้")]
        public string ContactAddress { get; set; }
        // อาชีพ
        [DisplayName("อาชีพปัจจุบัน")]
        [Required(ErrorMessage = "กรุณากรอกอาชีพปัจจุบัน")]
        public string Occupation { get; set; }
        [DisplayName("สถานที่ทำงาน")]
        [Required(ErrorMessage = "กรุณากรอกสถานที่ทำงาน")]
        public string Workplace { get; set; } // สถานที่ทำงาน / บริษัท

        // บุคคลใกล้ชิด
        [DisplayName("บุคคลใกล้ชิด")]
        [Required(ErrorMessage = "กรุณากรอกชื่อบุคคลใกล้ชิด")]
        public string ClosePersonName { get; set; } // ชื่อ-สกุล                                          
        [DisplayName("เบอร์โทรศัพท์บุคคลใกล้ชิด")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "กรูณากรอกเบอร์โทร์ 10 หลัก")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "เบอร์โทรศัพท์ต้องขึ้นต้นด้วย 0")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์บุคคลใกล้ชิด")]
        public string ClosePersonPhone { get; set; } // โทรศัพท์
        // ข้อมูลเพิ่มเติม
        [DisplayName("โรคประจำตัว")]
        [Required(ErrorMessage = "กรุณากรอกโรคประจำตัว")]
        public string MedicalCondition { get; set; } // โรคประจำตัว
        [DisplayName("สถานะโรคประจำตัว")]
        [Required(ErrorMessage = "กรุณากรอกสถานะโรคประจำตัว")]
        public string Status_Medical { get; set; } // โรคประจำตัว

        [DisplayName("อีเมล")]
        [EmailAddress(ErrorMessage = "กรุณากรอกอีเมลให้ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        public string Email { get; set; } // E-mail Address
        [DisplayName("ไอดีไลน์")]
        [Required(ErrorMessage = "กรุณากรอกไอดีไลน์")]
        public string LineId { get; set; }  // Line ID
        [DisplayName("วันที่สมัคร")]
        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; } = DateTime.Now.Date; // วันที่สมัคร
        [DisplayName("ผู้เเนะนำ")]
        [Required(ErrorMessage = "กรุณากรอกผู้เเนะนำ")]
        public string adviser { get; set; } // หมายเหตุ
        [DisplayName("หมายเหตู")]
        [Required(ErrorMessage = "กรุณากรอกหมายเหตู")]
        public string Remarks { get; set; } 
        [DisplayName("รูปโปรไฟล์")]
        public string? Images { get; set; } // รูปภาพ
        [DisplayName("รุ่นที่เรียน")]
        public int? ClassStudie_Id { get; set; }
        [ValidateNever]
        [ForeignKey("ClassStudie_Id")]
        public ClassStudied classStudied { get; set; }

        public double month1 { get; set; }
        public double month2 { get; set; }
        public double month3 { get; set; }
        public double month4 { get; set; }
        public double month5 { get; set; }
        public double month6 { get; set; }
        public double TotalScore
        {
            get
            {
                return month1 + month2 + month3 + month4 + month5 + month6;
            }
        }

        public double Total => TotalScore / 2;

        [DisplayName("คะแนนการเข้าเรียน")]
        public double? Attendance_Score { get; set; } // คะแนนการบ้าน
        [DisplayName("โอนหน่วยกิต")]
        public double? credit { get; set; } = 0;// หน่วยกิต
        public bool IsSelected { get; set; } = false;
       
    }
}
