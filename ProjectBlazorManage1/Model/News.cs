namespace ProjectBlazorManage1.Model
{
    public class News
    {
        [Key]
        public int News_Id { get; set; }
        [StringLength(200, ErrorMessage = "หัวข้อข่าวต้องไม่เกิน 200 ตัวอักษร")]
        [DisplayName("หัวข้อข่าว")]
        [Required(ErrorMessage = "กรุณากรอกหัวข้อข่าว")]
        public string Title { get; set; }
        [DisplayName("เนื้อหาข่าว")]
        [Required(ErrorMessage = "กรุณากรอกเนื้อหาข่าว")]
        public string News_Content { get; set; }
        [DisplayName("วันที่เผยเเพร่")]
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; } = DateTime.Now.Date;
        [DisplayName("รูปภาพ")]
        public string Images { get; set; }
        [DisplayName("วิดีโอ YouTube(ถ้ามี)")]
        [Url(ErrorMessage = "กรุณาระบุ URL ที่ถูกต้อง")]
        public string? YouTubeUrl { get; set; }
        [DisplayName("สถานะข่าว")]
        public bool Status { get; set; }
    }
}
