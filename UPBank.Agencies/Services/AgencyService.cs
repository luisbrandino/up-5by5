using Microsoft.AspNetCore.Mvc;
using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Agencies.Data;
using UPBank.DTOs;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.Services
{
    public class AgencyService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;

        public AgencyService(IEmployeeService employeeService, IAddressService addressService, IAccountService accountService)
        {
            _employeeService = employeeService;
            _addressService = addressService;
            _accountService = accountService;
        }

        public async void FillData(Agency agency)
        {
            agency.Employees = _employeeService.GetEmployeesByAgencyNumber(agency.Number).Result.ToList();
            agency.Address = await _addressService.GetAddressByZipcode(agency.AddressZipcode)?? new();
        }

        public async Task<IEnumerable<Account>> GetRestrictedAccounts(string agencyNumber) => await _accountService.GetRestrictedAccounts(agencyNumber);

        public async Task<IEnumerable<Account>> GetAccountsByProfile(string agencyNumber, EProfile profile) => await _accountService.GetAccountsByProfile(agencyNumber, profile);

        public async Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft(string agencyNumber) => await _accountService.GetAccountsWithActiveOverdraft(agencyNumber);

        //private async Task<IEnumerable<Account>> GetAccountsFromAgency(string agencyNumber) => await _accountService.GetRestrictedAccounts(agencyNumber);

        public async Task<Address?> GetAddress(AddressDTO dto)
        {
            var address = await _addressService.GetAddressByZipcode(dto.Zipcode);

            if (address == null)
                address = await _addressService.PostAddressFromDTO(dto);

            return address;
        }

        public async Task<Agency> CreateAgencyFromDTO(AgencyDTO dto)
        {
            var manager = new Employee
            {
                Cpf = dto.Manager.Cpf,
                BirthDate = dto.Manager.BirthDate,
                AgencyNumber = dto.Manager.AgencyNumber,
                Email = dto.Manager.Email,
                AddressZipcode = dto.Manager.AddressZipcode,
                Gender = dto.Manager.Gender,
                Name = dto.Manager.Name,
                Phone = dto.Manager.Phone,
                Manager = true,
                Salary = dto.Manager.Salary,
                Register = dto.Manager.Register,
            };

            return new Agency
            {
                Cnpj = dto.Cnpj,
                Number = dto.Number,
                Restriction = false,
                Employees = new() { _employeeService.PostManagerEmployee(manager).Result },
                Address = await GetAddress(dto.Address),
                AddressZipcode = dto.Address.Zipcode
            };
        }
    }
}