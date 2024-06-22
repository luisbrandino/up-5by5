# Documentação de Rotas - UPBank

## Endereços

### 1. Listar todos endereços

- **Endpoint**: `GET /api/addresses`
- **Descrição**: Retorna os endereços cadastrados.
- **Exemplo de Resposta**:
  ```json
    [
        {
            "zipcode": "15990-540",
            "number": "5",
            "street": "Avenida Trolesi",
            "neighborhood": "Jardim Buscardi",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        },
        { 
            "zipcode": "15990-640",
            "number": "5",
            "street": "Avenida Siqueira Campos",
            "neighborhood": "Nova Matão",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        },
        {
            "zipcode": "15990-740",
            "number": "5",
            "street": "Avenida José Gonçalves",
            "Neighborhood": "Jardim Pereira",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        }
    ]
  ```
  
### 2. Encontrar um endereço

- **Endpoint**: `GET /api/addresses/zipcode/:zipcode`
- **Descrição**: Retorna o endereço com base no código postal, se cadastrado.
- **Parâmetros**:
    - **zipcode**: string
- **Exemplo de Resposta**:
    ```json
    {
        "zipcode": "15990-540",
        "number": "5",
        "street": "Avenida Trolesi",
        "neighborhood": "Jardim Buscardi",
        "complement": "Casa A",
        "city": "Matão",
        "state": "SP"
    }
    ```
      
### 3. Cadastrar um endereço

- **Endpoint**: `POST /api/addresses`
- **Descrição**: Cadastra um novo endereço com base no código postal.
- **Corpo da requisição**:
    ```json
    {
        "zipcode": "15990-820",
        "number": "156",
        "complement": "Casa B" // Opcional
    }
    ```
- **Exemplo de Resposta**:
    ```json
    {
        "zipcode": "15990-820",
        "number": "156",
        "street": "Avenida Jornalista José da Costa Filho",
        "neighborhood": "Jardim Pereira",
        "complement": "Casa B",
        "city": "Matão",
        "state": "SP"
    }
    ```
