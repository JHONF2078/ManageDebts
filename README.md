# Instrucciones de Despliegue Local y Decisiones Técnicas

Esta sección describe cómo desplegar localmente la aplicación, que está compuesta por un backend en C# (.NET), un frontend en Angular, y servicios de Redis y PostgreSQL orquestados con Docker Compose.

## Despliegue Local

1. **Requisitos previos:**

   - Docker y Docker Compose instalados.
   - Visual Studio 2022 con .NET SDK Version 8.
   - Node.js y npm (para el frontend Angular).

2. **Configuración de servicios:**

   - El archivo `docker-compose.yml` define y levanta los servicios de PostgreSQL y Redis.
   - Las credenciales y parámetros de conexión (host, puerto, usuario, contraseña, etc.) se definen como variables de entorno en el servicio backend dentro de `docker-compose.yml`.
   - El backend en .NET toma estas variables de entorno para construir la cadena de conexión a PostgreSQL y Redis en tiempo de ejecución, permitiendo una configuración flexible y segura.

3. **Pasos para levantar el entorno:**
   - Ejecuta `docker compose up -d` para iniciar los servicios de base de datos y redis.
   - Inicia el backend seleccionando el perfil `ManageDebts.Api` en Visual Studio y presionando el botón de ejecutar (play ▶️).
   - Instala dependencias y ejecuta el frontend Angular:
     ```bash
     cd frontend/ManageDebts.Web
     npm install
     npm start
     ```

# Preguntas de Arquitectura y Experiencia

## Microservicios

Si el sistema creciera y necesitara pasar de monolito a microservicios, dividiría los servicios en:

- **Servicio de usuarios/autenticación**
- **Servicio de gestión de deudas**
- **Servicio de pagos**
- **Servicio de notificaciones**

Consideraciones de comunicación:

- Usar API Gateway para centralizar el acceso.
- Comunicación entre servicios vía HTTP REST o gRPC.
- Implementar resiliencia y manejo de fallos en las llamadas HTTP/gRPC usando Polly (reintentos, circuit breaker, timeout, etc.).
- Implementar mensajería asíncrona (ej. RabbitMQ) para eventos críticos.

## Optimización en la nube (AWS)

- **Autenticación segura:** Usaría AWS Cognito para gestión de usuarios y autenticación.
- **Base de datos:** Amazon RDS por su alta disponibilidad y backups automáticos.
- **Cache y escalabilidad:** Amazon ElastiCache (Redis) para cache distribuido y escalable.
- **Balanceo de carga:** AWS Elastic Load Balancer para distribuir tráfico entre instancias.

## Buenas prácticas de seguridad

1. **Backend:** Validación de entradas, uso de JWT, y protección contra inyección SQL usando consultas parametrizadas y EF .
2. **Frontend:** Sanitización de datos, uso de HTTPS, y control de acceso a las rutas(guards).
3. **Despliegue en la nube:** Uso de secrets (AWS Secrets Manager), redes privadas (VPC), y monitoreo de logs.

## PostgreSQL vs NoSQL

- **PostgreSQL:** Usaría en escenarios donde se requieren transacciones, relaciones complejas y consultas SQL (ej. sistema financiero, gestión de usuarios).
- **NoSQL:** Usaría en aplicaciones con datos semi-estructurados, alta escalabilidad horizontal y baja necesidad de relaciones (ej. chat en tiempo real, almacenamiento de logs), es decir donde los datos se han simples y en gran volumen de datos.

## Despliegue (CI/CD)

AWS CodePipeline

1. **Integración continua:**
   - Build y test automáticos en cada push usando AWS CodeBuild integrado en CodePipeline.
2. **Testeo:**
   - Ejecución de pruebas unitarias y de integración en CodeBuild.
3. **Despliegue continuo:**
   - Deploy automático a entornos de staging y producción usando AWS CodeDeploy, ECS (para contenedores) o EKS (Kubernetes), todo orquestado desde CodePipeline.
4. **Control de calidad:**
   - Análisis estático de código, revisión de dependencias y validaciones automáticas en las etapas del pipeline.
