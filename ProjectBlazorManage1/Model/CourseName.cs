namespace ProjectBlazorManage1.Model
{
    public class CourseName
    {
        public int Id { get; set; }
        [DisplayName("ชื่อหลักสูตร")]
        [Required(ErrorMessage = "กรุณากรอกชื่อหลักสูตร")]
        public string Name { get; set; }
        [DisplayName("ลำดับหลักสูตร")]
        public int Level { get; set; } 
        public bool Status { get; set; } 
    }
}
