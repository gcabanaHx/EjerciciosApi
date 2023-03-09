using API.Models;
using RestSharp;
using System;
using System.Net;
using Tests.Helpers;
using Xunit;

namespace Tests.TestCases
{
    public class TestHxMinimalAPI : BaseTest
    {
        private const string adminsEndpoint = "/admins";
        private const string credentials = "admin:8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

        public HttpStatusCode statusCode;

        [Theory]
        [InlineData(2, "AdminPrueba", "db5d7daabe02d4cb3aba20613e93da853efdca88ffe00181834e5c577ee213d8")]
        public void Get_QueryAdminByID_AdminDataIsCorrect(int id, string username, string passHash)
        {
            var admin = client.Get<Admin>($"{adminsEndpoint}/{id}",
                headers: Data.GetAuthorizationHeader(credentials));


            Assert.NotNull(admin);
            Assert.Equal(admin.UserName, username);
            Assert.Equal(admin.PassHash, passHash);
        }

        [Theory]
        [InlineData("newAdmin", "1234")]
        public void Post_CreateNewAdmin_AdminDataIsCorrect(string username, string passHash)
        {
            var payload = new { username, passHash };

            var admin = client.Post<Admin>(adminsEndpoint, payload,
                headers: Data.GetAuthorizationHeader(credentials));


            Assert.Equal(admin.UserName, username);
            Assert.Equal(admin.PassHash.Length, 64);

        }


        // PARA TODOS LOS EJERCICIOS

        // Authorization: Basic YWRtaW46OGQ5NjllZWY2ZWNhZDNjMjlhM2E2MjkyODBlNjg2Y2YwYzNmNWQ1YTg2YWZmM2NhMTIwMjBjOTIzYWRjNmM5Mg==
        // http://localhost:9999/admins
        // http://localhost:9999/users

        /**
         * Para todos los casos, incluir pruebas sin autenticación para validar que los
         * métodos no son accesibles sin los permisos adecuados. Asimismo, incluir pruebas
         * negativas para validar los tipos de datos enviados en cada payload (cuando sea posible).
         **/

        // GET

        /**
         * EJ 1
         * Validar que al obtener los datos del user admin el código de respuesta sea 200.
         *
         **/

        /**
         * EJ 2
         * Validar que al obtener los datos de un usuario,
         * la respuesta contenga los datos de todos los usuarios (id, name, lastName, phoneNumber, email).
         * Como adicional, validar que la estructura de la respuesta sea correcta (JSON schema).
         *
         **/

        // POST

        /**
         * EJ 3
         * Crear un nuevo user. Validar que los datos de usuario enviados correspondan
         * con los del body de la respuesta. Posteriormente con el ID obtenido,
         * validar mediante un GET que el registro creado anteriormente haya sido
         * persistido correctamente y ubicado al final de la lista de usuarios.
         **/

        /**
         * EJ 4
         * Validar que enviando un payload malformado o algún tipo de dato incorrecto o fuera de rango,
         * el servicio responde con un error 400 Bad Request
         **/

        // PUT

        /**
         * EJ 5
         * Actualizar el telefono de un usuario con un número mayor al permitido (mayor a 2147483647).
         * Validar que el servicio responde con un mensaje de error.
         **/

        /**
         * EJ 6
         * Actualizar todos los datos de un usuario. Validar que los datos del usuario enviado
         * correspondan con los del body de la respuesta. Adicionalmente, validar mediante un
         * GET que los datos actualizados anteriormente hayan sido persistidos correctamente.
         **/

        // DELETE

        /**
         * EJ 7
         * Validar que al intentar borrar un registro inexistente el servicio responde con un mensaje de error.
         * Luego borrar un registro existente y validar que el response code es 204 - No Content.
         **/

        /**
         * EJ 8
         * Validar mediante un GET que el registro borrado anteriormente haya sido eliminado correctamente.
         **/

        /**
         * EJ 9
         * Mediante el uso del endpoint deleteAll, eliminar todos los usuarios persistidos.
         **/

        /**
         * EJ 10
         * Validar mediante un GET que todos los regitros de usuarios han sido eliminados de la persistencia.
         **/
    }
}