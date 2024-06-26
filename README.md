<div align="left">

# **Guia de Uso da API de Funcionários**

## **HTTP Function: Get**  
*(GetEmployee)*  
<sub>
Descrição: Usada para pegar JSON de todos os Funcionários cadastrados  
Endpoint: `https://localhost:7028/api/Employees/`  
Parâmetros: -  
Endpoints consumidos: get(all) Agência e get(all) Endereço  
Body Sample: -  
</sub>

---

## **HTTP Function: Get(id)**  
*(GetEmployeeById)*  
<sub>
Descrição: Usada para pegar JSON de um funcionário específico por sua chave primária (CPF)  
Endpoint: `https://localhost:7028/api/Employees/{cpf}`  
Parâmetros: {cpf} (padrão numérico apenas)  
Endpoints consumidos: get(all) Agência e get(all) Endereço  
Body Sample: -  
</sub>

---

## **HTTP Function: Post**  
*(HireEmployee)*  
<sub>
Descrição: Adiciona um funcionário ao DB de funcionários, com base no body inserido, não permitindo adicionar CPFs inválidos   
Endpoint: `https://localhost:7028/api/Employees/Hire  
Parâmetros: -  
Endpoints consumidos: get(all) Agência e get(all) Endereço  
Body Sample:  
</sub>

```json
{
  "Cpf": "61235000079",
  "Name": "João Silva",
  "Gender": "M",
  "Salary": 5000.00,
  "Phone": "11987654321",
  "Email": "joao.silva@example.com",
  "BirthDate": "2000-01-01",
  "AddressZipcode": "14805-295",
  "Manager": true,
  "AgencyNumber": "001"
}
 ```

## **Http Function:  Patch**    
*(AlterAccount)*
<sub>  
Descrição: A partir de uma inserção de body para alteração de uma conta, através dos acessos dos funcionários (comum / gerente). A opção "SetProfile" permite que um funcionário padrão define o tipo da conta a partir do salário do cliente (universitária, comum ou vip), enquanto a opção "ApproveAccount" permite que um gerente possa ativar uma conta inativa ou desativar uma conta ativa  
Endpoint: https://localhost:7028/api/Employees/{optionType}  
Parâmetros: optionType pode ser "SetProfile" ou "ApproveAccount"  
Endpoints consumidos: get(id) Agencia, get(id) Conta e get(id) Funcionário  
Body Sample:  
</sub>

 ```json
         {
            "Restriction": false,
            "AgencyNumber": "001",
            "CustomerCPF": "65076248024",
            "EmployeeCPF": "84113873054",
            "AccountNumber":"1234"
         }
 ```
## **Http Function:  Delete**
*(DeleteEmployee)*
<sub>  
Descrição: Move um registro de funcionário da tabela funcionários para a tabela de funcionários deletados (desligados)  
Endpoint: https://localhost:7028/api/Employees/  
Parâmetros: {cpf} (padrão numérico apenas)  
Endpoints consumidos: Employee  
Body Sample: -
</sub>
