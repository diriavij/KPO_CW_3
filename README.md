# Контрольная работа по КПО №3  
**Асинхронное межсервисное взаимодействие**

В этом решении реализованы 3 микросервиса:

- **OrdersService** — приём и хранение заказов, публикация события о запросе платежа.  
- **PaymentsService** — обработка платежей, публикация результата оплаты.  
- **APIGateways** — рутинг запросов.

Кроме того, в проекте используется:
- **RabbitMQ** ― брокер сообщений.  
- **PostgreSQL** ― реляционная база данных для хранения данных микросервисов.  
- **MassTransit.EntityFrameworkCoreOutbox** ― реализация transactional outbox/inbox.  
- **MediatR** + **EF Core** для внутри­сервисной бизнес-логики.

---
- В проекте корректно реализованы Dockerfile и docker-compose.yml, коллекция Swagger.

- Применены паттерны Transactional Outbox в Order Service, Transactional Inbox и Outbox в Payments Service. Обеспечивается семантики exactly once при списании денег у пользователя.
---

## Запуск инфраструктуры

В корне проекта есть `docker-compose.yml`, который поднимает:
- **PostgreSQL**: порт **5432**  
- **RabbitMQ**: порт **5672** (AMQP), **15672** 
- **OrdersService**: порт **5001**  
- **PaymentsService**: порт **5002**
- **APIGateway** порт **8080**

```bash
docker-compose up --build
