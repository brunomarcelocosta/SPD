using SPD.MVC.Geral.Hubs;
using System;
using System.Globalization;

namespace SPD.MVC.Geral.Utilities
{
    public sealed class Notification
    {
        public Guid ID { get; set; }
        public string Message { get; set; }
        public NotificationHub.NotificationType Type { get; set; }
        public bool UseAnimation { get; set; }
        public NotificationHub.NotificationFor For { get; set; }
        public int UserID { get; set; }
        public long Countdown { get; set; }
        public long CountdownLimit { get; set; }
        public bool AlteracaoPerfil { get; set; }


        public Notification(long countdownLimit)
        {
            this.ID = Guid.NewGuid();
            this.Message = String.Empty;
            this.Type = NotificationHub.NotificationType.INFORMATION;
            this.UseAnimation = false;
            this.For = NotificationHub.NotificationFor.Nobody;
            this.UserID = -1;
            this.Countdown = -1;
            this.CountdownLimit = countdownLimit;
        }

        public void SynchronizeCountdown(long ticks)
        {
            this.Countdown = this.CountdownLimit - ticks;

            NotificationHub.Notify(this);
        }

        public string ToString(bool alteracaoPerfil)
        {
            var totalSeconds = Convert.ToInt64(TimeSpan.FromTicks(this.Countdown).TotalSeconds);
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);

            if (totalSeconds > 60)
            {
                if (alteracaoPerfil)
                {
                    string tempo = t.Minutes.ToString() + " minuto(s) e " + t.Seconds.ToString() + " segundos.";

                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, this.Message, tempo));
                }
                else
                {
                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, "{0} Restam aproximadamente {1} minuto(s) e {2} segundos.", this.Message, t.Minutes, t.Seconds));
                }

            }
            if (totalSeconds > 5)
            {
                if (alteracaoPerfil)
                {
                    string tempo = t.Seconds.ToString() + " segundos.";

                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, this.Message, tempo));
                }
                else
                {
                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, "{0} Restam aproximadamente {1} segundos.", this.Message, totalSeconds));
                }

            }
            else if ((totalSeconds > 1) && (totalSeconds < 6))
            {
                if (alteracaoPerfil)
                {
                    string tempo = t.Seconds.ToString() + " segundos.";

                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, "Você será desconectado em instantes. Processando evento..."));
                }
                else
                {
                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, "{0} Processando evento...", this.Message));
                }
            }
            else
            {
                if (alteracaoPerfil)
                {
                    return String.Format(CultureInfo.InvariantCulture, String.Format(CultureInfo.InvariantCulture, "Você será desconectado. Processando evento..."));
                }

                return String.Format(CultureInfo.InvariantCulture, this.Message);
            }
        }


    }
}
