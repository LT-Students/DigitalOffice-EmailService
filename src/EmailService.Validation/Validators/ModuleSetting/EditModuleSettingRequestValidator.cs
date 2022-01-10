using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.EmailService.Validation.Validators.ModuleSetting.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EmailService.Validation.Validators.ModuleSetting
{
  public class EditModuleSettingRequestValidator : BaseEditRequestValidator<EditModuleSettingRequest>, IEditModuleSettingRequestValidator
  {
    private static Regex PasswordRegex = new(@"(?=.*[.,:;?!*+%\-<>@[\]{}/\\_{}$#])");

    private void HandleInternalPropertyValidation(Operation<EditModuleSettingRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditModuleSettingRequest.Host),
          nameof(EditModuleSettingRequest.Port),
          nameof(EditModuleSettingRequest.EnableSsl),
          nameof(EditModuleSettingRequest.Email),
          nameof(EditModuleSettingRequest.Password)
        });

      AddСorrectOperations(nameof(EditModuleSettingRequest.Host), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditModuleSettingRequest.Port), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditModuleSettingRequest.EnableSsl), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditModuleSettingRequest.Email), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditModuleSettingRequest.Password), new List<OperationType> { OperationType.Replace });

      #endregion

      #region Host

      AddFailureForPropertyIf(
        nameof(EditModuleSettingRequest.Host),
        x => x == OperationType.Replace,
        new()
        {
          { x => string.IsNullOrWhiteSpace(x.value?.ToString().Trim()), "Host must not be empty." },
        });

      #endregion

      #region Port

      AddFailureForPropertyIf(
        nameof(EditModuleSettingRequest.Port),
        x => x == OperationType.Replace,
        new()
        {
          { x => int.TryParse(x.value?.ToString(), out int _), "Incorrect format of Port." },
        });

      #endregion

      #region EnableSsl

      AddFailureForPropertyIf(
        nameof(EditModuleSettingRequest.EnableSsl),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect format of EnableSsl." },
        });

      #endregion

      #region Email

      AddFailureForPropertyIf(
        nameof(EditModuleSettingRequest.Email),
        x => x == OperationType.Replace,
        new()
        {
          { x => 
            {
              try
              {
                MailAddress address = new(x.value?.ToString().Trim());
                return true;
              }
              catch
              {
                return false;
              }
            }, "Incorrect email address."
          },
        });

      #endregion

      #region Password

      AddFailureForPropertyIf(
        nameof(EditModuleSettingRequest.Password),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Password must not be empty." },
          { x => x.value.ToString().Length >= 8, "Password is too short." },
          { x => x.value.ToString().Length <= 14, "Password is too long." },
          { x => PasswordRegex.IsMatch(x.value.ToString()), "The password must contain at least one special character." },
          { x => !x.value.ToString().Contains(' '), "Password must not contain space." }
        });

      #endregion
    }

    public EditModuleSettingRequestValidator()
    {
      RuleForEach(x => x.Operations).Custom(HandleInternalPropertyValidation);
    }
  }
}
