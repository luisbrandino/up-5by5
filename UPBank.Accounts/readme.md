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
    - **number**: string
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

### 3. Listar todas as contas

- **Endpoint**: GET /api/accounts/restricted
- **Descrição**: Retorna todas as contas que estão restritas.
- **Parâmetros**: Nenhum.
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

- **Endpoint**: GET /api/Accounts/activeloans
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