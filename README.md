## Guia Finger Web API 3.0 (.NET 6.0)

#### Neste guia foi utilizado o endpoint local: https://localhost:7202;
#### Testamos as rotas com o software Insomnia; API rodando direto no Visual Studio 2022 (C#).


# <h3>Rotas</h3>

# <h4>/login (POST)</h4>

Para acessar as demais rotas, devemos nos autenticar e não receber 401 como resposta.
Envie o seguinte JSON no corpo da requisição POST:
 
Se estiver correto, obterá a resposta:
 
Pegue a string de “Token” e coloque em todas as requisições a partir daqui:

###### A autenticação da API foi feita com JSON Web Token (JWT).

# <h4>/capture (GET)</h4>
 
Rota GET responsável por capturar a digital, tendo saída um ID e o hash da digital.
Retorno em JSON após acessar a rota e inserir a digitar no leitor Hamster III:

# <h4>/enroll (GET)</h4>
 
Rota GET responsável por capturar múltiplas digitais, tendo saída um ID e o hash das digitais.
Necessário inserir o /{ID} do enroll (print exemplo acima).

# <h4>/verify-match (GET)</h4>

Rota GET responsável por comparar uma digital com outra (1:1), deve se fornecer uma hash para comparação:
 
###### O retorno desta rota é booleano (ou erro).

# <h4>/identify (GET)</h4>

Rota GET responsável por comparar uma digital com múltiplas (1:N), mas pode ser modificado para 1:1 também; ideal para identificar digitais com um banco de dados:
 
No exemplo acima, a digital capturada corresponde ao usuário de ID 2.
O JSON enviado nessa rota é um vetor de objetos.

##### Observação: Fique atento a ortografia das chaves dos objetos.

# <h3>Geral</h3>

O usuário deve sempre ter o leitor (modelos da Nitgen) com os drivers instalados e funcionais.
A API deve ser programada e moldada para o negócio do usuário.

# <h3>JSON Web Token (JWT)</h3>

Esta API utiliza da JWT para autenticação e autorização das rotas;
Os usuários para acesso aos tokens, ficam na classe UserRepository.cs, pode ser trocado para um banco de dados; 
Nas classes User.cs e TokenService.cs, é possível ativar roles para autorizações e tempo de expiração dos tokens.

# <h4>Troca de porta da API</h4>

No arquivo JSON: appsettings.json é possível trocar as portas da aplicação:
 
Atenção: No exemplo acima, a porta 5050 será usada tanto no DEBUG, como no DEPLOY.
