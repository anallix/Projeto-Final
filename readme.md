# 🍽️ Sistema de Reservas & Gestão de Restaurante

Bem-vindo ao repositório oficial do projeto. Este é um sistema completo desenvolvido em **.NET 8** para gerenciamento de reservas, controle de clientes e administração de mesas, utilizando uma arquitetura moderna que separa Back-end (API) e Front-end (MVC).

![Status do Projeto](https://img.shields.io/badge/Status-Concluído-brightgreen) ![Testes](https://img.shields.io/badge/Testes-5%2F5%20Passing-success) ![Net Version](https://img.shields.io/badge/.NET-8.0-blue)

---

## 📚 Documentação Completa

Abaixo você encontra todos os artefatos de engenharia e manuais do projeto.

| Tipo | Documento | Descrição |
| :--- | :--- | :--- |
| 📖 | **[Manual do Usuário](docs/manual-usuario.md)** | Guia de uso do sistema. |
| 🏗️ | **[Documentação Técnica](docs/documentacao-tecnica.md)** | Arquitetura e tecnologias. |
| 📋 | **[Regras de Negócio (BRD)](docs/regras-negocio.pdf)** | Requisitos de Negócio Detalhados. |
| 📄 | **[Especificação (ERS)](docs/especificacao-requisitos.pdf)** | Especificação de Requisitos de Software. |
| 📊 | **[Diagrama do Banco (DER)](docs/diagrama-banco.pdf)** | Modelagem de dados. |
| 🔄 | **[Casos de Uso](docs/casos-uso.pdf)** | Diagrama de funcionalidades. |
| 🖼️ | **[Diagrama de Sequência](docs/diagrama-sequencia.png)** | Fluxo visual de reserva (Imagem). |
| 📅 | **[Planilha de Planejamento](SEU_LINK_DO_EXCEL)** | Cronograma (Excel). |

---

## 🚀 Tecnologias Utilizadas

O projeto foi construído com ferramentas robustas de mercado:

* **Linguagem:** C# (.NET 8)
* **Back-end (API):** ASP.NET Core Web API
* **Front-end (Site):** ASP.NET Core MVC com Razor Views
* **Banco de Dados:** SQLite (com Entity Framework Core)
* **Qualidade:** xUnit (Testes Unitários Automatizados)
* **Documentação:** Swagger / OpenAPI

---

## 📂 Estrutura do Projeto

O sistema é dividido em três módulos principais:

1.  **`swagger/` (API):**
    * O "cérebro" do sistema. Contém Controllers, Regras de Negócio e acesso ao Banco.
    * Documentação viva disponível via Swagger UI.

2.  **`ReservaFront/` (Interface):**
    * A interface do usuário (MVC).
    * Consome os dados da API via HTTP Client.

3.  **`ReservaApi.Tests/` (Qualidade):**
    * Projeto de testes unitários que valida as regras de negócio e integridade da API.

---

## 🧪 Testes Automatizados

Garantimos a qualidade do código através de testes unitários.
Atualmente, o sistema conta com cobertura para os principais fluxos:

* ✅ CRUD de Clientes
* ✅ Validações de Regra de Negócio
* ✅ Tratamento de Erros (404/500)

Para rodar os testes:
```bash
dotnet test