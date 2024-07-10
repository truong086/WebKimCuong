using CsvHelper;
using OfficeOpenXml;
using System.Formats.Asn1;
using System.Globalization;
using thuongmaidientus1.Common;

namespace thuongmaidientus1.AccountService
{
    public interface IExcelService
    {
        Task<bool> UpdateCSVData(List<Student> students);
    }

    public class ExcelService : IExcelService
    {
        public async Task<bool> UpdateCSVData(List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                return false;
            }

            try
            {
                string _filePath = "C:\\Users\\ASUS\\OneDrive\\Desktop\\PythongImportFile\\dataUser.csv";

                // Mở file CSV hoặc tạo file mới nếu file chưa tồn tại
                /*
                    "StreamWriter" được sử dụng để tạo hoặc mở file CSV để ghi dữ liệu. Đối số thứ hai của "StreamWriter" là một 
                    biến boolean true, nó cho phép ghi thêm vào cuối file nếu file đã tồn tại.
                 */
                using (var writer = new StreamWriter(_filePath, true))

                /*
                    "CsvWriter" từ thư viện CsvHelper được sử dụng để ghi dữ liệu theo định dạng CSV. 
                    "CultureInfo.InvariantCulture" được sử dụng để xác định cách xử lý các định dạng số và ngôn ngữ trong file CSV.
                 */
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Nếu file CSV chưa có dữ liệu, viết header
                    /*
                        Đoạn mã kiểm tra xem file CSV đã tồn tại chưa và có dữ liệu không. Nếu file không tồn tại 
                        hoặc có kích thước bằng 0, nghĩa là chưa có dữ liệu trong file, một header sẽ được viết vào file CSV 
                        bằng cách sử dụng "csv.WriteHeader<Student>()". "csv.NextRecord()" sau đó di chuyển con trỏ đến dòng tiếp theo 
                        để chuẩn bị ghi dữ liệu.
                     */
                    var fileInfo = new FileInfo(_filePath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                    {
                        /*
                            - GIẢI THÍCH KỸ DÒNG LỆNH "csv.WriteHeader<Student>();"
                            * Dòng lệnh "csv.WriteHeader<Student>();" trong thư viện CsvHelper được sử dụng để viết header của file CSV 
                              dựa trên các thuộc tính của lớp "Student". Header này chứa tên của các cột trong file CSV.
                            * Khi gọi "csv.WriteHeader<Student>();", thư viện CsvHelper sẽ xác định các thuộc tính public
                              của lớp "Student" và sử dụng chúng để tạo header cho file CSV. Mỗi thuộc tính sẽ được sử dụng 
                              làm tên cột tương ứng trong file CSV.
                            * Ví dụ, nếu lớp "Student" có các thuộc tính như "Id, Name, Age, và GPA" 
                              dòng lệnh "csv.WriteHeader<Student>();" sẽ viết header cho file CSV với các tên cột
                              lần lượt là "Id", "Name", "Age", và "GPA".
                            * Khi header được viết vào file CSV, các dòng dữ liệu tiếp theo (được ghi thông qua "csv.WriteRecord(student);") 
                              sẽ tương ứng với các thuộc tính của đối tượng Student và được sắp xếp theo thứ tự tương ứng với header 
                              đã được viết. Điều này giúp cho việc đọc và hiểu cấu trúc của file CSV trở nên dễ dàng và rõ ràng hơn.
                         */
                        csv.WriteHeader<Student>();
                        csv.NextRecord();
                    }


                    // Ghi dữ liệu của từng sinh viên vào file CSV
                    /*
                        Trong vòng lặp foreach, mỗi đối tượng sinh viên (Student) từ danh sách students được ghi vào file CSV 
                        bằng "csv.WriteRecord(student)". csv.NextRecord() di chuyển con trỏ đến dòng tiếp theo để chuẩn bị ghi dữ liệu
                        của sinh viên tiếp theo.
                     */
                    foreach (var student in students)
                    {
                        /*
                            - GIẢI THÍCH KỸ DÒNG LỆNH "csv.WriteRecord(student);"
                        * Khi gọi csv.WriteRecord(student);, thư viện sẽ sử dụng các quy tắc mặc định hoặc quy tắc bạn đã cấu hình 
                          để chuyển đổi thông tin từ đối tượng Student thành một dòng trong file CSV. Các thuộc tính của 
                          đối tượng "Student" sẽ được trích xuất và ghi ra theo định dạng CSV.

                        * Ví dụ, nếu đối tượng "Student" có các thuộc tính như "Id, Name, Age, và GPA", khi gọi "csv.WriteRecord(student);", 
                         thư viện sẽ ghi các giá trị tương ứng của các thuộc tính này thành một dòng trong file CSV. Đối với mỗi đối tượng 
                         "Student" trong danh sách, một dòng mới sẽ được thêm vào file CSV.
                         */
                        csv.WriteRecord(student);
                        csv.NextRecord();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Xử lý exception hoặc có thể log lỗi ở đây
                return false;
            }
        }

    }
    public class Student
    {
        public string? CustomerID { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
        public string? AnnualIncome { get; set; }
        public string? SpendingScore { get; set; }
        // Thêm các thuộc tính khác của sinh viên tại đây
    }

}
