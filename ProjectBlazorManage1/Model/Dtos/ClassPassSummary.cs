namespace ProjectBlazorManage1.Model.Dtos
{
    public class ClassPassSummary
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int TotalStudents { get; set; }
        public int PassedStudents { get; set; }
        public double PassPercentage { get; set; }
        public int PassedStudentsWithCertificate { get; set; }

    }
}
