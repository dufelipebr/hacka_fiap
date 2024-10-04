# Health&Med - Entrega Hackathon .NET - FIAP POSTECH

## Descrição do projeto
Esse projeto é o trabalho de conclusão Hackathon .NET da FIAP PosTech grupo 27.<br>
O objetivo é resolver o problema proposto que é a criação de um sistema para agendamento de consultas médicas, onde era manual e agora deve ser digital, foi criado um MPV para demostrar a solução. 

## :hammer: Funcionalidades do projeto
- `Login`<br>
Permite a autenticação do usuário e a geração do token de acesso.<br>

- `Cadastro do Médico`<br>
Criação do médico no sistema<br>

- `Criar Agenda Médica`<br>
Criação da agenda do médico no sistema<br>

- `Alteração da Agenda Médica`<br>
Alteração da agenda do médico no sistema, médico so pode alterar caso não tenha consulta marcada.<br>

- `Listagem da Agenda Médica`<br>
Listagem da agenda do médico no sistema, o médico deve passar o crm para que liste apenas as suas consultas<br>

- `Cadastro do Paciente`<br>
Criação do paciente no sistema<br>

- `Listar Agenda do Médico`<br>
Listagem da agenda do médico no sistema, paciente deve passar o CRM do médico e apenas será listadas os horários não marcados, caso tenha concorrencia na marcação a primeira será marcada e a segunda recebera uma mensagem "Horario já reservado".<br>

- `Agendamento de consultas`<br>
O paciente escolhe o horário desejado para marcar a consulta, caso tenha concorrência na marcação a primeira será marcada e a segunda recebera uma mensagem "Horario já reservado".<br>

## Tecnologias utilizadas
- C# .NET 8.
- MongoDB.

## Pessoas Desenvolvedoras do Projeto:
### Grupo 27
- Alexandre Cruz da Costa Maciel.
- Carlos Eduardo Felipe de Oliveira.

## Licença:
- Gratuita
