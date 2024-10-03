namespace apibronco.bronco.com.br.Entity
{
    public class AgendaTime
    {
        public AgendaTime(DateTime date)
        {
            this.Data = new DateTime(date.Year, date.Month, date.Day);
            this.Hora = date.Hour;
            this.Minutos = date.Minute;
            //this.HoraFinal = date.Hour + 1;
        }
        public AgendaTime(DateTime date, int horaInicio, int horaFim)
        {

        }
        public DateTime Data { get; set; }
        public int Hora { get; set; }
        public int Minutos { get; set; }

        public string GetDateFormatInicial()
        {
            return Data.ToString("yyyy-mm-dd") + " " + Hora + ":" + Minutos;
        }

        public DateTime GetDate()
        {
            return new DateTime(Data.Year, Data.Month, Data.Day, Hora, Minutos, 0);
        }
    }
}
