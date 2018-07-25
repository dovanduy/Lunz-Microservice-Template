using System;

namespace Lunz.Microservice.ReferenceData.Models.Api
{
    /// <summary>
    /// 从哪里听说
    /// </summary>
    public class HearFromDetails
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}