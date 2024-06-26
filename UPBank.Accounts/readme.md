# Documentação de Rotas - UPBank

## Contas

### 1. Listar todas as contas

- **Endpoint**: GET /api/accounts
- **Descrição**: Retorna todas as contas cadastradas.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
  
```json
[
  {
        "number": "031262",
        "restriction": false,
        "overdraft": 1040,
        "profile": 1,
        "creationDate": "2024-06-25T17:12:38.7307024",
        "balance": 1900,
        "savingsAccount": null,
        "creditCard": {
            "number": 43841637301934,
            "extractionDate": "2034-06-25T17:12:39.8554956",
            "limit": 500,
            "cvv": "218",
            "holder": "Test",
            "brand": "Visa",
            "active": true
        },
        "agency": {
            "number": "10001",
            "cnpj": "101.202.2021/22",
            "restriction": false,
            "employees": [],
            "address": null
        },
        "transactions": [
            {
                "id": 1,
                "effectiveDate": "2024-06-25T17:49:23.8795207",
                "type": 2,
                "value": 100,
                "origin": null,
                "destiny": null
            }
        ],
        "customers": [
            {
                "cpf": "000.000.000-01",
                "name": "Test",
                "birthDate": "2004-06-26T10:36:48.424264-03:00",
                "gender": "M",
                "salary": 1500.99,
                "phone": "16999998888",
                "email": "test@test.com",
                "address": null
            }
        ]
    }
  ...
]
```

### 2. Encontrar uma conta

- **Endpoint**: GET /api/accounts/{number}
- **Descrição**: Retorna uma conta com base no número da conta.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
```json
    {
        "number": "031262",
        "restriction": false,
        "overdraft": 1040,
        "profile": 1,
        "creationDate": "2024-06-25T17:12:38.7307024",
        "balance": 1900,
        "savingsAccount": null,
        "creditCard": {
            "number": 43841637301934,
            "extractionDate": "2034-06-25T17:12:39.8554956",
            "limit": 500,
            "cvv": "218",
            "holder": "Test",
            "brand": "Visa",
            "active": true
        },
        "agency": {
            "number": "10001",
            "cnpj": "101.202.2021/22",
            "restriction": false,
            "employees": [],
            "address": null
        },
        "transactions": [
            {
                "id": 1,
                "effectiveDate": "2024-06-25T17:49:23.8795207",
                "type": 2,
                "value": 100,
                "origin": null,
                "destiny": null
            }
        ],
        "customers": [
            {
                "cpf": "000.000.000-01",
                "name": "Test",
                "birthDate": "2004-06-26T10:36:48.424264-03:00",
                "gender": "M",
                "salary": 1500.99,
                "phone": "16999998888",
                "email": "test@test.com",
                "address": null
            }
        ]
    }
```

### 3. Listar todas as contas restritas por agência

- **Endpoint**: GET /api/accounts/agency/{number}/restricted
- **Descrição**: Retorna todas as contas que estão restritas.
- **Parâmetros**:
    - **number**: string
- **Exemplo de Resposta**:
```json
[
  {
        "number": "031262",
        "restriction": true,
        "overdraft": 1040,
        "profile": 1,
        "creationDate": "2024-06-25T17:12:38.7307024",
        "balance": 1900,
        "savingsAccount": null,
        "creditCard": {
            "number": 43841637301934,
            "extractionDate": "2034-06-25T17:12:39.8554956",
            "limit": 500,
            "cvv": "218",
            "holder": "Test",
            "brand": "Visa",
            "active": true
        },
        "agency": {
            "number": "10001",
            "cnpj": "101.202.2021/22",
            "restriction": false,
            "employees": [],
            "address": null
        },
        "transactions": [],
        "customers": [
            {
                "cpf": "000.000.000-01",
                "name": "Test",
                "birthDate": "2004-06-26T10:36:48.424264-03:00",
                "gender": "M",
                "salary": 1500.99,
                "phone": "16999998888",
                "email": "test@test.com",
                "address": null
            }
        ]
    }
  ...
]
```

### 4. Listar todas as contas

- **Endpoint**: GET /api/accounts/profile/{profile}
- **Descrição**: Retorna todas as contas com base no perfil.
- **Parâmetros**:
    - **profile**: number (1 até 3)
        - 1. Universitary;
        - 2. Normal;
        - 3. Vip
- **Exemplo de Resposta**:
```json
{
    "number": "031262",
    "restriction": true,
    "overdraft": 1040,
    "profile": 2,
    "creationDate": "2024-06-25T17:12:38.7307024",
    "balance": 1900,
    "savingsAccount": null,
    "creditCard": {
        "number": 43841637301934,
        "extractionDate": "2034-06-25T17:12:39.8554956",
        "limit": 500,
        "cvv": "218",
        "holder": "Test",
        "brand": "Visa",
        "active": true
    },
    "agency": {
        "number": "10001",
        "cnpj": "101.202.2021/22",
        "restriction": false,
        "employees": [],
        "address": null
    },
    "transactions": [],
    "customers": [
        {
            "cpf": "000.000.000-01",
            "name": "Test",
            "birthDate": "2004-06-26T10:36:48.424264-03:00",
            "gender": "M",
            "salary": 1500.99,
            "phone": "16999998888",
            "email": "test@test.com",
            "address": null
        }
    ]
}
```

### 5. Listar contas com empréstimos ativos

- **Endpoint**: GET /api/accounts/activeloans
- **Descrição**: Retorna todas as contas que possuem empréstimos ativos.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
```json
[
  {
        "number": "031262",
        "restriction": false,
        "overdraft": 1040,
        "profile": 1,
        "creationDate": "2024-06-25T17:12:38.7307024",
        "balance": 1900,
        "savingsAccount": null,
        "creditCard": {
            "number": 43841637301934,
            "extractionDate": "2034-06-25T17:12:39.8554956",
            "limit": 500,
            "cvv": "218",
            "holder": "Test",
            "brand": "Visa",
            "active": true
        },
        "agency": {
            "number": "10001",
            "cnpj": "101.202.2021/22",
            "restriction": false,
            "employees": [],
            "address": null
        },
        "transactions": [
            {
                "id": 1,
                "effectiveDate": "2024-06-25T17:49:23.8795207",
                "type": 3, // Loan
                "value": 100,
                "origin": null,
                "destiny": null
            }
        ],
        "customers": [
            {
                "cpf": "000.000.000-01",
                "name": "Test",
                "birthDate": "2004-06-26T10:36:48.424264-03:00",
                "gender": "M",
                "salary": 1500.99,
                "phone": "16999998888",
                "email": "test@test.com",
                "address": null
            }
        ]
    }
  ...
]
```

### 6. Listar transações de uma conta por tipo

- **Endpoint**: GET /api/accounts/{number}/transactions/{type}
- **Descrição**: Retorna todas as transações de uma conta baseado no tipo da transação
- **Parâmetros**:
    - **number**: string,
    - **type**: number (1 até 5)
        - 1. Withdraw;
        - 2. Deposit;
        - 3. Loan;
        - 4. Transfer;
        - 5. Payment
- **Exemplo de Resposta**:
```json
[
    {
        "id": 1010,
        "effectiveDate": "2024-06-26T10:04:47.4547432",
        "type": 2,
        "value": 500,
        "origin": {
            "number": "085787",
            "restriction": false,
            "overdraft": 500,
            "profile": 1,
            "creationDate": "2024-06-26T09:22:11.8877561",
            "balance": 28100,
            "savingsAccount": null,
            "creditCard": {
                "number": 43841637301934,
                "extractionDate": "2034-06-26T09:22:11.922793",
                "limit": 500,
                "cvv": "089",
                "holder": "Test",
                "brand": "Visa",
                "active": false
            },
            "agency": {
                "number": "10001",
                "cnpj": "101.202.2021/22",
                "restriction": false,
                "employees": [],
                "address": null
            },
            "transactions": null,
            "customers": [
                {
                    "cpf": "000.000.000-01",
                    "name": "Test",
                    "birthDate": "2004-06-26T11:09:51.81775-03:00",
                    "gender": "M",
                    "salary": 1500.99,
                    "phone": "16999998888",
                    "email": "test@test.com",
                    "address": null
                }
            ]
        },
        "destiny": null
    }
]
```

### 7. Consultar extrato da conta

- **Endpoint**: GET /api/accounts/{number}/statement
- **Descrição**: Retorna todas as transações da conta
- **Parâmetros**:
    - **number**: string
- **Exemplo de Resposta**:
```json
[
    {
        "id": 1002,
        "effectiveDate": "2024-06-26T09:52:12.7530797",
        "type": 2,
        "value": 100,
        "origin": {
            "number": "083021",
            "restriction": true,
            "overdraft": 500,
            "profile": 1,
            "creationDate": "2024-06-26T09:50:57.6729527",
            "balance": 5100,
            "savingsAccount": null,
            "creditCard": {
                "number": 42382585635779,
                "extractionDate": "2034-06-26T09:50:58.8725018",
                "limit": 500,
                "cvv": "430",
                "holder": "Test",
                "brand": "Visa",
                "active": false
            },
            "agency": {
                "number": "10001",
                "cnpj": "101.202.2021/22",
                "restriction": false,
                "employees": [],
                "address": null
            },
            "transactions": [],
            "customers": [
                {
                    "cpf": "000.000.000-01",
                    "name": "Test",
                    "birthDate": "2004-06-26T11:31:55.6014684-03:00",
                    "gender": "M",
                    "salary": 1500.99,
                    "phone": "16999998888",
                    "email": "test@test.com",
                    "address": null
                }
            ]
        },
        "destiny": null
    },
    ...
]
```

### 8. Consultar saldo da conta

- **Endpoint**: GET /api/accounts/{number}/balance
- **Descrição**: Retorna o saldo atual da conta especificada
- **Parâmetros**:
    - **number**: string
- **Exemplo de Resposta**:
```json
{
    "Balance": 4000
}
```

### 9. Alterar uma conta

- **Endpoint**: PUT /api/accounts/{number}
- **Descrição**: Altera os dados de uma conta com as informações passadas no corpo da requisição
- **Corpo da requisição**:
```json
{
    "balance": 2200, // opcional
    "overdraft": 1500 // opcional
}
```
- **Exemplo de Resposta**:
```json
{
    "number": "031262",
    "restriction": false,
    "overdraft": 1500,
    "profile": 1,
    "creationDate": "2024-06-25T17:12:38.7307024",
    "balance": 2200,
    "savingsAccount": null,
    "creditCard": null,
    "agency": null,
    "transactions": null,
    "customers": null
}
```

### 10. Alterar perfil da conta

- **Endpoint**: PUT /api/accounts/{number}/profile
- **Descrição**: Altera o perfil da conta com o perfil passado no corpo da requisição
- **Corpo da requisição**:
```json
{
    "profile": 2 // de 1 até 3
}
```
- **Exemplo de resposta:**
```json
{
    "number": "031262",
    "restriction": false,
    "overdraft": 1500,
    "profile": 2,
    "creationDate": "2024-06-25T17:12:38.7307024",
    "balance": 2200,
    "savingsAccount": null,
    "creditCard": null,
    "agency": null,
    "transactions": null,
    "customers": null
}
```

### 10. Cadastrar uma conta

- **Endpoint**: POST /api/accounts
- **Descrição**: Cria uma nova conta com base nas informações passadas no corpo da requisição
- **Corpo da requisição**:
```json
{
    "Overdraft": 100,
    "Profile": 1, // 1 até 3
    "AgencyNumber": "10001"
    "IsSavingsAccount": false // verdadeiro se for conta poupança, falso se não for
    "Customers": [
        "000.000.000-01"
    ] // apenas CPF
}
```
- **Exemplo de resposta:**
```json
{
    "number": "031262",
    "restriction": false,
    "overdraft": 1500,
    "profile": 2,
    "creationDate": "2024-06-25T17:12:38.7307024",
    "balance": 2200,
    "savingsAccount": null,
    "creditCard": null,
    "agency": null,
    "transactions": null,
    "customers": null
}
```
