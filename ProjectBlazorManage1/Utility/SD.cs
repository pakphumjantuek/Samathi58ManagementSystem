namespace ProjectBlazorManage1.Utility
{
    public static class SD
    {
        public static string Role_Admin = "Admin";
        public static string Role_Customer = "ผู้ใช้งานทั่วไป";

        public enum DegreeTypes
        {
            ปริญญาตรี = 1,  // ปริญญาตรี
            ปริญญาโท = 2,     // ปริญญาโท
            ปริญญาเอก = 3,
            อื่นๆ = 4// ปริญญาเอก
        }
        public enum DegreeName
        {
            วิทยาศาสตรบัณฑิต = 1,  // ปริญญาตรี
            ครุศาสตรบัณฑิต = 2,     // ปริญญาโท
            ศิลปศาสตรบัณฑิต = 3,
            บริหารธุรกิจบัณฑิต = 4// ปริญญาเอก
        }

        public enum StudiedType
        {
            [Display(Name = "มาเรียน")]
            มาเรียน = 1,

            [Display(Name = "เรียนเสริม")]
            เรียนเสริม = 2,

            [Display(Name = "เรียนต่างสาขา")]
            เรียนต่างสาขา = 3,

            [Display(Name = "ขาดเรียน")]
            ขาดเรียน = 4,

            [Display(Name = "อื่นๆ")]
            อื่นๆ = 5
        }
        public enum DegreeType
        {
            [Display(Name = "ปริญญาตรี")]
            ปริญญาตรี = 1,

            [Display(Name = "ปริญญาโท")]
            ปริญญาโท = 2,

            [Display(Name = "ปริญญาเอก")]
            ปริญญาเอก = 3,

            [Display(Name = "อื่นๆ")]
            อื่นๆ = 4
        }
        public enum RelationshipType
        {
            คู่สมรส = 1,      
            พ่อแม่ = 2,      
            ลูก = 3,         
            พี่น้อง = 4,   
            ปู่ย่าตายาย = 5,
            หลาน = 6,     
            ลุง,ป้า,น้า,อา = 7,    
            ลูกพี่ลูกน้อง = 9,  
            เพื่อน = 10, 
            ผู้ปกครอง = 11, 
            อื่นๆ = 12   

        }

    }

}
