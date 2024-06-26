# Route Documentation - UPBank

## Agencies

### 1. get agencies

- **Endpoint**: `GET /api/agencies`
- **Description**: Returns registered agencies.
- **Sample**:
  ```json
    [
        {
            "number": "001",
            "cnpj": "12345678901234",
            "restriction": false,
            "employees": [
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
            }],
            "address": {
                "zipcode": "15990-540",
                "number": "5",
                "street": "Avenida Trolesi",
                "neighborhood": "Jardim Buscardi",
                "complement": "Casa A",
                "city": "Matao",
                "state": "SP"
            }
        },
        {
            "number": "002",
            "cnpj": "14246918302251",
            "restriction": true,
            "employees": [
            {
                "Cpf": "67288933246",
                "Name": "Pedro Augusto",
                "Gender": "M",
                "Salary": 4500.00,
                "Phone": "16996634251",
                "Email": "pedrogusto@example.com",
                "BirthDate": "1982-02-11",
                "AddressZipcode": "14803-521",
                "Manager": true,
                "AgencyNumber": "002"
            }],
            "address": {
                "zipcode": "14804-240",
                "number": "5",
                "street": "Avenida Trolesi",
                "neighborhood": "Jardim Buscardi",
                "complement": "Predio A",
                "city": "Araraquara",
                "state": "SP"
            }
        }
    ]
  ```
  
### 2. Get agency by number

- **Endpoint**: `GET /api/agencies/agency/agencynumber`
- **Description**: Returns a agency by agency number, if registered.
- **Parameters**:
    - **agencynumber**: string
- **Sample**:
    ```json
    {
            "number": "001",
            "cnpj": "12345678901234",
            "restriction": false,
            "employees": [
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
            }],
            "address": {
                "zipcode": "15990-540",
                "number": "5",
                "street": "Avenida Trolesi",
                "neighborhood": "Jardim Buscardi",
                "complement": "Casa A",
                "city": "Matao",
                "state": "SP"
            }
        }
    ```
      
### 3. Get restricted accounts

- **Endpoint**: `GET /api/agencies/agencynumber/restricteds`
- **Description**: Returns a restricted accounts list from requested agency number.
- **Parameters**:
    - **agencynumber**: string
- **Sample**:
    ```
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

### 4. Get accounts by profile

- **Endpoint**: `GET /api/agencies/agencynumber/byprofile/profile`
- **Description**: Returns a accounts list from requested profile and agency number.
- **Parameters**:
    - **agencynumber**: string
- **Sample**:
    ```
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

### 5. Get accounts with active overdraft

- **Endpoint**: `GET /api/agencies/agencynumber/byprofile/profile`
- **Description**: Returns a accounts list from requested profile and agency number.
- **Parameters**:
    - **agencynumber**: string,
    - **profile**: EProfile(Normal, University, Vip)
- **Sample**:
    ```
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

### 6. Put agency

- **Endpoint**: `PUT /api/agencies/agencynumber`
- **Description**: Try to validate and then update agency data.
- **Parameters**:
    - **agencynumber**: string,
- **Request body**:
    ```json
        {
            "cnpj": "12345678901234",
            "employee":
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
            "address": {
                "zipcode": "15990-540",
                "number": "5",
                "complement": "Casa A",
            }
        }
    ```
- **Sample**:
    - NoContent

### 7. Post agency

- **Endpoint**: `POST /api/agencies/`
- **Description**: Try to validate and then create a new agency, if it exists in deleted agencies context, remove too.
- **Parameters**:
    - **agencynumber**: string
- **Request body**:
    ```json
        {
            "Number": "001",
            "cnpj": "12345678901234",
            "restriction": false,
            "Manager": {
                "CPF": "61235000079",
                "Name": "João Silva","
                "BirthDate": "2000-01-01",
                "Gender": 'M',
                "Salary": 5000.00,"
                "Phone": "11987654321",
                "Email": "joaosilva@example.com"
                "Register": "1",
                "AgencyNumber": "001",
                "Manager": true,"
                "AddressZipcode": "14805-295",
            }
            "address": {
                "zipcode": "15990-540",
                "number": "5",
                "complement": "Casa A",
            }
        }
    ```
- **Sample**
    ```json 
        {
            "number": "001",
            "cnpj": "12345678901234",
            "restriction": false,
            "employees": [
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
            }],
            "address": {
                "zipcode": "15990-540",
                "number": "5",
                "street": "Avenida Trolesi",
                "neighborhood": "Jardim Buscardi",
                "complement": "Casa A",
                "city": "Matao",
                "state": "SP"
            }
        }
    ```

### 8. Delete agency
- **Endpoint**: `DELETE /api/agencies/agencynumber`
- **Description**: Delete a agency by agency number, if registered, and saves in deleted agencies context.
- **Parameters**:
	- **agencynumber**: string
- **Sample**:
	- NoContent

### 9. Patch agency restriction
- **Endpoint**: `PATCH /api/agencies/agencynumber/`
- **Description**: Verify if agency exists and update restriction status.
- **Parameters**:
	- **agencynumber**: string
- **Sample**:
	- NoContent