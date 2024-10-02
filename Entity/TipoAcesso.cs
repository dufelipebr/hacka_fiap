namespace apibronco.bronco.com.br.Entity
{
    public enum EnumTipoAcesso
    {
        Pacionte = 1, 
        Medico = 2,
    }

    public static class Permissoes
    {
        public const string Paciente = "Paciente";
        public const string Medico = "Medico";
    }
}
