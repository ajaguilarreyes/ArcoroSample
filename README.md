# ArcoroSample

## Proceso para ingresar los secrets
### Paso 1 habilitar los secrets para el proyecto

~~~
 dotnet user-secrets init --project ./ArcoroSamples/ArcoroSamples.csproj 
~~~

### Paso 2 Agregar las claves con el comando

~~~
 dotnet user-secrets set "Key" "Value"
~~~

### Paso 3 Agregar los valores para las siguientes claves
- ExternalProviders:HH2:ApiKey
- ExternalProviders:HH2:ApiSecret
- ClientCredentials:{Subdomain}:Username
- ClientCredentials:{Subdomain}:Password

