namespace ChatHelpdescAgent
{
    public class Client
    {
        /// <summary>
        /// <param name="id"> Идентификатор клиента</param>
        /// <param name="name">Имя клиента/Номер телефона</param>
        /// <param name="comment">Комментарий к клиенту</param>
        /// <param name="phone">Номер телефона клиента</param>
        /// <param name="avatar">Ссылка на аватарку</param>
        /// <param name="assigned_name">Псевдоним клиента</param>
        /// <param name="region_id">Идентификатор региона</param>
        /// <param name="external_id">Внешний идентификатор(телеграмм и др.)</param>
        /// <param name="extra_comment_1">Поле 1</param>
        /// <param name="extra_comment_2">Поле 2</param>
        /// <param name="extra_comment_3">Поле 3</param>
        /// </summary>
        public  string id { get; set; }
        public string name { get; set; }
        public string comment { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public string assigned_name { get; set; }
        public string region_id { get; set; }
        public string country_id { get; set; }
        public string external_id { get; set; }
        public string extra_comment_1 { get; set; }
        public string extra_comment_2 { get; set; }
        public string extra_comment_3 { get; set; }
        public string [] transports { get; set; }
    }
}
