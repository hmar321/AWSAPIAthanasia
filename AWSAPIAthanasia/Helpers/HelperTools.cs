namespace ApiAthanasia.Helpers
{
    public enum EstadosPedido
    {
        Espera = 1, Procesando = 2, Enviado = 3, EnRuta = 4,
        Entregado = 5, Devolucion = 6, Cancelado = 7, Finalizado = 8
    }
    public enum Estados { Activo = 1, Pendiente = 2, Eliminado = 3 }
    public enum Roles { Admin = 1, Cliente = 2 }
    public enum Formatos { TapaDura = 0, TapaBlanda = 1, Ebook = 2 }
    public class HelperTools
    {
        public static string GetFormato(Formatos formato)
        {
            string dato = "";
            switch (formato)
            {
                case Formatos.TapaDura:
                    dato = "Tapa Dura";
                    break;
                case Formatos.TapaBlanda:
                    dato = "Tapa Blanda";
                    break;
                case Formatos.Ebook:
                    dato = "Ebook";
                    break;
            }
            return dato;
        }

        public static int GetFormatoId(Formatos formato)
        {
            int dato = -1;
            switch (formato)
            {
                case Formatos.TapaDura:
                    dato = 1;
                    break;
                case Formatos.TapaBlanda:
                    dato = 2;
                    break;
                case Formatos.Ebook:
                    dato = 3;
                    break;
            }
            return dato;
        }

        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 38; i++)
            {
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        public static bool CompareArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }

        public static string GenerateTokenMail()
        {
            Random random = new Random();
            string token = "";
            for (int i = 1; i <= 14; i++)
            {
                int aleat;
                do
                {
                    aleat = random.Next(65, 122);
                } while (aleat > 90 && aleat < 97);
                token += Convert.ToChar(aleat);
            }
            return token;
        }

        public static int GetRolId(Roles rol)
        {
            int dato = -1;
            switch (rol)
            {
                case Roles.Admin:
                    dato = 1;
                    break;
                case Roles.Cliente:
                    dato = 2;
                    break;
            }
            return dato;
        }

        public static int GetEstadoId(Estados estado)
        {
            int dato = -1;
            switch (estado)
            {
                case Estados.Activo:
                    dato = 1;
                    break;
                case Estados.Pendiente:
                    dato = 2;
                    break;
                case Estados.Eliminado:
                    dato = 3;
                    break;
            }
            return dato;
        }
        public static int GetEstadoPedidoId(EstadosPedido estado)
        {
            int dato = -1;
            switch (estado)
            {
                case EstadosPedido.Espera:
                    dato = 1;
                    break;
                case EstadosPedido.Procesando:
                    dato = 2;
                    break;
                case EstadosPedido.Enviado:
                    dato = 3;
                    break;
                case EstadosPedido.EnRuta:
                    dato = 4;
                    break;
                case EstadosPedido.Entregado:
                    dato = 5;
                    break;
                case EstadosPedido.Devolucion:
                    dato = 6;
                    break;
                case EstadosPedido.Cancelado:
                    dato = 7;
                    break;
                case EstadosPedido.Finalizado:
                    dato = 8;
                    break;
            }
            return dato;
        }
    }
}