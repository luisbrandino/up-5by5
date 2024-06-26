# Documentação de Rotas - UPBank

## Clientes

### 1. Listar todos Clientesa

- **Endpoint**: `GET /api/customers`
- **Descrição**: Retorna os clientes cadastrados.
- **Exemplo de Resposta**:
 ```json
    [
    {
        "restriction": true,
        "cpf": "460.673.258-02",
        "name": "Luan",
        "birthDate": "1999-06-24T00:00:00",
        "gender": "M",
        "salary": 100000,
        "phone": "((1) 2) 34-567-",
        "email": "luan@example.com",
        "address": {
            "zipcode": "15990-640",
            "number": 5,
            "complement": "",
            "street": "Avenida Siqueira Campos",
            "city": "Matão",
            "state": "SP",
            "neighborhood": "Nova Matão"
        }
    },
    {
        "restriction": true,
        "cpf": "753.952.270-40",
        "name": "Roger",
        "birthDate": "2005-05-15T00:00:00",
        "gender": "M",
        "salary": 12000,
        "phone": "(16) 98839-6290",
        "email": "joao.silva@example.com",
        "address": {
            "zipcode": "15990-640",
            "number": 5,
            "complement": "",
            "street": "Avenida Siqueira Campos",
            "city": "Matão",
            "state": "SP",
            "neighborhood": "Nova Matão"
        }
    }
]
```

### 2. Encontrar um Cliente

- **Endpoint**: `GET /api/customers/:cpf`
- **Descrição**: Retorna o cliente com base no endereço, se cadastrado.
- **Parâmetros**:
    - **cpf**: string
- **Exemplo de Resposta**:
    ```json
    {
    "restriction": true,
    "cpf": "753.952.270-40",
    "name": "Roger",
    "birthDate": "2005-05-15T00:00:00",
    "gender": "M",
    "salary": 12000,
    "phone": "(16) 98839-6290",
    "email": "joao.silva@example.com",
    "address": {
        "zipcode": "15990-640",
        "number": 5,
        "complement": "",
        "street": "Avenida Siqueira Campos",
        "city": "Matão",
        "state": "SP",
        "neighborhood": "Nova Matão"
    }
  }
    ```

    ### 3. Cadastrar um cliente

- **Endpoint**: `POST /api/customers`
- **Descrição**: Cadastra um novo cliente no banco de dados.
- **Corpo da requisição**:
    ```json
    {
        "Cpf": "577.406.150-72",
        "Name": "Claúdio",
        "BirthDate": "2001-05-15",
        "Gender": "M",
        "Salary": 12000.00,
        "Phone": "(16)98839-6290",
        "Email": "Claudio@example.com",
        "Address": "14807-031",
         "AddressNumber": 300
        

    }
    ```
- **Exemplo de Resposta**:
    ```json
   {
    "restriction": true,
    "cpf": "57740615072",
    "name": "Claúdio",
    "birthDate": "2001-05-15T00:00:00",
    "gender": "M",
    "salary": 12000,
    "phone": "16988396290",
    "email": "Claudio@example.com",
    "address": {
        "zipcode": "14807-031",
        "number": 300,
        "complement": "",
        "street": "Avenida Siqueira Campos",
        "city": "Matão",
        "state": "SP",
        "neighborhood": "Nova Matão"
       }
  }
    ```

     ### 4. Editar um cliente

- **Endpoint**: `PUT /api/customers/:cep`
- **Descrição**: Editar um cliente no banco de dados.
- **Corpo da requisição**:
    ```json
    {
        "Cpf": "577.406.150-72",
        "Name": "Claúdio",
        "BirthDate": "2001-05-15",
        "Gender": "M",
        "Salary": 20000.00,
        "Phone": "(16)98839-6290",
        "Email": "Claudio@example.com",
        "Address": "15990-640"
        "AddresNumber": 500
    }
    ```
- **Exemplo de Resposta**:
    204 No Content


    ### 5. Deletar um cliente

- **Endpoint**: `PUT /api/customers/:cep`
- **Descrição**:Deletar um cliente (Jogar para a tabela de Clientes Deletados).
- **Exemplo de Resposta**:
    ```json
   {
    "restriction": true,
    "cpf": "400.893.500-22",
    "name": "Claúdio",
    "birthDate": "2001-05-15T00:00:00",
    "gender": "M",
    "salary": 12000,
    "phone": "(16) 98839-6290",
    "email": "Claudio@example.com",
    "address": {
        "zipcode": "15990-640",
        "number": 5,
        "complement": "",
        "street": "Avenida Siqueira Campos",
        "city": "Matão",
        "state": "SP",
        "neighborhood": "Nova Matão"
    }
}
    ```

  ### 6. Mudar Restrição de um cliente

- **Endpoint**: `PATCH /api/customers/:cep/changerestriction`
- **Descrição**:Deletar um cliente (Jogar para a tabela de Clientes Deletados).
- **Exemplo de Resposta**:
      204 No Content

