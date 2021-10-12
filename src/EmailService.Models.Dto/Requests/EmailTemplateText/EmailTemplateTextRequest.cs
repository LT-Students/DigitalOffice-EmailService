﻿using System;

namespace LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate
{
  public record EmailTemplateTextRequest
  {
    public Guid? EmailTemplateId { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
    public string Language { get; set; }
  }
}