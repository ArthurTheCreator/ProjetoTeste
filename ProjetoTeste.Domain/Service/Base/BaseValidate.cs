﻿using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Base.ApiResponse;
using ProjetoTeste.Arguments.Enum.Validate;
using ProjetoTeste.Domain.Helper;
using ProjetoTeste.Domain.Notification;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ProjetoTeste.Domain.Service.Base;

public class BaseValidate<TValidateDTO> where TValidateDTO : BaseValidateDTO
{

    #region Base
    public static List<TValidateDTO> RemoveInvalid(List<TValidateDTO> listValidateDTO) => (from i in listValidateDTO where !i.Invalid select i).ToList();
    public static List<TValidateDTO> RemoveIgnore(List<TValidateDTO> listValidateDTO) => (from i in listValidateDTO where !i.Ignore select i).ToList();
    #endregion

    #region Validate

    #region InvalidLength
    public static EnumValidateType InvalidLenght(string? value, int minLeght, int MaxLenght)
    {
        if (string.IsNullOrWhiteSpace(value))
            return minLeght == 0 ? EnumValidateType.Valid : EnumValidateType.NonInformed;

        int lenght = value.Length;

        if (lenght > MaxLenght)
            return EnumValidateType.Invalid;

        return EnumValidateType.Valid;
    }
    #endregion

    #region CPFValidate
    public static EnumValidateType CPFValidate(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return EnumValidateType.NonInformed;

        cpf = Regex.Replace(cpf, "[^0-9]", string.Empty); //Substitui todo q não é digito por uma string vazia Ex = 123.456.789-09

        if (cpf.Length != 11) return EnumValidateType.Invalid;

        if (new string(cpf[0], cpf.Length) == cpf) return EnumValidateType.Invalid; // Verifica se o cpf é composto pelo primeiro digito repetido

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += (cpf[i] - '0') * multiplicadores1[i]; // (cpf[i] - '0') -> jeito de converter um caracter em um digito numerico (substitui o valor unico) ex '0' = valor unicode 48 -> '0': = 48 - 48 = 0
        }

        int resto = soma % 11;
        int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto; // (operador ternário)  Verifica o primeiro digito verificador se é menor que dois, ser for é igual a 0, se for maior que dois a conta é (11 - resto)

        if (cpf[9] - '0' != primeiroDigitoVerificador) return EnumValidateType.Invalid; //verificar se o o primeiro digito veriicador é igual o primeiro

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += (cpf[i] - '0') * multiplicadores2[i];
        }

        resto = soma % 11;
        int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

        if (cpf[10] - '0' != segundoDigitoVerificador) return EnumValidateType.Invalid;

        return EnumValidateType.Valid;
    }
    #endregion

    #region EmailValidate
    public static EnumValidateType EmailValidate(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email)) return EnumValidateType.NonInformed;
        try
        {
            var validate = new MailAddress(email);
            return EnumValidateType.Valid;
        }
        catch (FormatException)
        {
            return EnumValidateType.Invalid;
        }
    }
    #endregion

    #region PhoneValidate
    public static EnumValidateType PhoneValidate(string phone)
    {
        if (string.IsNullOrEmpty(phone) || string.IsNullOrWhiteSpace(phone)) return EnumValidateType.NonInformed;
        phone = phone.Replace(" ", "").Replace("+", "").Replace("-", "");
        if (phone.Length > 10 && phone.Length < 16 && phone.All(char.IsDigit)) return EnumValidateType.Valid;
        return EnumValidateType.Invalid;
    }

    #endregion

    #endregion

    #region MessageGeneration

    #region Error
    public bool InvalidNull(int index)
    {
        return AddErrorMessage(index.ToString(), NotificationMessage.InvalidNull(index));
    }

    public bool InvalidLenght(string identifier, string? value, int minLeght, int maxLenght, EnumValidateType validateType, string propertyName)
    {
        return HandleValidation(identifier, validateType, NotificationKey.InvalidLenghtKey, NotificationMessage.InvalidLenght(propertyName, value, minLeght, maxLenght), NotificationKey.NullFieldKey, NotificationMessage.NullField(propertyName));
    }

    public bool RepeatedIdentifier(string identifier, string fieldName)
    {
        return AddErrorMessage(identifier, NotificationMessage.RepeatedIdentifier(fieldName, identifier));
    }

    public bool AlreadyExists(string identifier, string fieldName)
    {
        return AddErrorMessage(identifier, NotificationMessage.AlreadyExists(fieldName, identifier));
    }

    public bool NotFoundId(string idetifier, string className, long id)
    {
        return AddErrorMessage(idetifier, NotificationMessage.NotFoundId(className, id));
    }

    public bool RepeatedId(string idetifier, long id)
    {
        return AddErrorMessage(idetifier, NotificationMessage.RepeatedId(id));
    }

    public bool LikedValue(string idetifier, string linkedValue, string className)
    {
        return AddErrorMessage(idetifier, NotificationMessage.LikedValue(className, linkedValue));
    }

    public bool InvalidCPF(string idetifier, string cpfValue)
    {
        return AddErrorMessage(idetifier, NotificationMessage.InvalidCPF(cpfValue));
    }

    public bool InvalidEmail(string idetifier, string emailValue)
    {
        return AddErrorMessage(idetifier, NotificationMessage.InvalidEmail(emailValue));
    }

    public bool InvalidPhone(string idetifier, string phoneValue)
    {
        return AddErrorMessage(idetifier, NotificationMessage.InvalidPhone(phoneValue));
    }

    public bool NegativeStock(string idetifier, long stock)
    {
        return AddErrorMessage(idetifier, NotificationMessage.NegativeStock(stock));
    }

    public bool NegativePrice(string idetifier, decimal price)
    {
        return AddErrorMessage(idetifier, NotificationMessage.NegativePrice(price));
    }

    public bool InvalidRelatedProperty(string idetifier, string fieldName, long relatedId)
    {
        return AddErrorMessage(idetifier, NotificationMessage.InvalidRelatedProperty(fieldName, relatedId));
    }
    #endregion

    #region Success
    public bool SuccessfullyRegistered(string idetifier, string className)
    {
        return AddSuccessMessage(idetifier, NotificationMessage.SuccessfullyRegistered(className, idetifier));
    }

    public bool SuccessfullyUpdated(string idetifier, long id, string className)
    {
        return AddSuccessMessage(idetifier, NotificationMessage.SuccessfullyUpdated(className, idetifier, id));
    }

    public bool SuccessfullyDeleted(string idetifier, string className)
    {
        return AddSuccessMessage(idetifier, NotificationMessage.SuccessfullyDeleted(className, idetifier));
    }
    #endregion

    #endregion

    #region Helper
    private bool AddToDictionary(string key, DetailedNotification detailedNotification)
    {
        NotificationHelper.Add(key, detailedNotification);
        return true;
    }

    public bool AddErrorMessage(string key, string message)
    {
        return AddToDictionary(key, new DetailedNotification(key, [message], EnumNotificationType.Error));
    }

    public bool AddSuccessMessage(string key, string message)
    {
        return AddToDictionary(key, new DetailedNotification(key, [message], EnumNotificationType.Success));
    }


    private bool HandleValidation(string key, EnumValidateType validateType, string KeyInvalid, string invalidMessage, string KeyNonInformed, string nonInformedMessage)
    {
        if (EnumValidateType.Invalid == validateType)
        {
            AddToDictionary(key, new DetailedNotification(key, [invalidMessage], EnumNotificationType.Error));
            return true;
        }
        if (EnumValidateType.NonInformed == validateType)
        {
            AddToDictionary(key, new DetailedNotification(key, [nonInformedMessage], EnumNotificationType.Error));
            return true;
        }
        return false;
    }

    public (List<DetailedNotification> Successes, List<DetailedNotification> Error) GetValidationResult()
    {
        var successes = NotificationHelper.Get().Where(i => i.NotificationType == EnumNotificationType.Success).ToList();
        var errors = NotificationHelper.Get().Where(i => i.NotificationType != EnumNotificationType.Success).ToList();

        return (successes, errors);
    }
    #endregion

}