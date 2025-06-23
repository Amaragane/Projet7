namespace P7CreateRestApi.DTO.CurveDTO
{
    public class GetUpdateCurvePointDTO
    {
        public int Id { get; set; }
        public byte? CurveId { get; set; }
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
    }
}
