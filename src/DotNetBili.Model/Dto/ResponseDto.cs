namespace DotNetBili.Model.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public int Code { get; set; } = 200;
        public string? Message { get; set; } = "";
        public object? Data { get; set; } = null;
        public ResponseDto()
        {
            
        }
        public ResponseDto(object data)
        {
            this.Data = data;
        }
    }
}
