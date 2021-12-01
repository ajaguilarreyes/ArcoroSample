# ArcoroSample

## Proceso para ingresar los secrets
### Paso 1 habilitar los secrets para el proyecto

~~~
 dotnet user-secrets init --project ./ArcoroSamples/ArcoroSamples.csproj 
~~~

### Paso 2 Agregar las claves

~~~
 dotnet user-secrets set "Key" "Value"
~~~

