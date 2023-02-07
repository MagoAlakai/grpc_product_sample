## The file extensions represent different things in the SSL configurations

- *.csr
-- This is the Certificate Request, and in this example above, where we create both the Certificate Request (*.csr) and the Encrypted Private Key (*.key) at the same time.

- *.key
-- This is the encrypted private key, don't share this. It is used later to produce the *.pfx file.

- *.pem
-- Unencrypted/Plain RSA Private key, don't share this one either. The *.pem is used to sign the *.crt file

- *.crt
-- This is the Public Certificate used by clients wishing to connect to a service. It can be used in a PSK setup. It is used later to produce the *.pfx file.

- *.pfx
-- Combined public and private key data, don't share this one either. This is used by the service to distribute the public key. The private key in the *.pfx file is used to decrypt the incoming encrypted data, which has been encrypted by the distributed public key.

## Install OpenSSL and enable working in PowerShell with it
```bash
winget install -e --id ShiningLight.OpenSSL
```
```bash
$env:path = $env:path + ";C:\Program Files\OpenSSL-Win64\bin"
```

## Create Certificate Request (*.csr) and Encrypted Private Key (*.key) // note: we could create our root before hand. CSR: Certificate Signing Request

```bash
openssl req -new -out productgrpc.internal.local.csr -keyout productgrpc.internal.local.key \-subj '/CN=productgrpc.internal.local' -extensions EXT -config <( \
  printf "[dn]\nCN=productgrpc.internal.local\n[req]\ndistinguished_name = dn\n[EXT]\nsubjectAltName=DNS:productgrpc.internal.local\nkeyUsage=digitalSignature\nextendedKeyUsage=serverAuth")
```

### PEM Pass Phrase: productgrpc.internal.local

### Two files generated is the `*.csr` and `*.key`
CSR: Certificate Signing Request
Next step is to "self-sign" our certificate signing request


## Use the *.key file to create the mathmatically linked Unencrypted RSA Private Key (*.pem), to "self-sign" our certificate signing request(.csr)

```bash
openssl rsa -in productgrpc.internal.local.key -out productgrpc.internal.local.pem
```

## Use the Certificate Request (*.csr) and the Unencrypted RSA Private Key (*.pem) to produce our Public Certificate (*.crt)

```bash
openssl x509 -in productgrpc.internal.local.csr -out productgrpc.internal.local.crt -req -signkey productgrpc.internal.local.pem -days 5475
```

## Checking public key (optional):
```bash
openssl pkey -inform PEM -pubin -in productgrpc.internal.local.pub -noout
openssl rsa -in productgrpc.internal.local.pem -pubout > productgrpc.internal.local.pub
openssl pkey -inform PEM -pubin -in productgrpc.internal.local.pub
```

## To generate a pfx file use the following command, password required:

```bash
openssl pkcs12 -export -out productgrpc.internal.local.pfx -inkey productgrpc.internal.local.key -in productgrpc.internal.local.crt
```

## Check contents of the *.pfx file

```bash
openssl pkcs12 -info -in productgrpc.internal.local.pfx
```


## Get thumbprint for the *.crt file, it should match the thumbprint of the *.pfx file // SHA1 Fingerprint

```bash
openssl x509 -noout -fingerprint -sha1 -inform pem -in productgrpc.internal.local.crt
```


## Get the thumbprint of the *.pfx file, it should match the thumbprint of the *.crt file / SHA1 Fingerprint

```bash
openssl pkcs12 -in productgrpc.internal.local.pfx -nodes | openssl x509 -noout -fingerprint
```

OR

```bash
openssl pkcs12 -in productgrpc.internal.local.pfx -nodes -passin pass:%1 | openssl x509 -noout -fingerprint
```

## Check if the server is running

```bash
openssl s_client -showcerts -connect productgrpc.internal.local:53445
```

## Sample appsettings-json entry for Kestrel HTTP2

```json
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2",
      "SslProtocols": [ "Tls12", "Tls13" ],
      "ClientCertificateMode": "AllowCertificate"
    },
    "Endpoints": {
      "Https": {
        "Url": "https://productgrpc.internal.local:53443",
        "Port": 53443,
        "Certificate": {
          "Path": "Certificates/productgrpc.internal.local.pfx",
          "Password": "productgrpc.internal.local",
          "AllowInvalid": false
        }
      }
    }
  }
```


## Export the *.pfx into a base64 encoded content, to use in as part of the CI/CD variable set

```bash
openssl base64 -in productgrpc.internal.local.pfx -out productgrpc.internal.local.pfx.b64
```


## To convert base64 encoded *.pxf.b64 (text file) back into *.pfx (binary file)

```bash
openssl base64 -d -in productgrpc.internal.local.pfx.b64 -out productgrpc.internal.local.pfx
```

## More checks (Optional)

### Export the private key file from the pfx file
```bash
openssl pkcs12 -in productgrpc.internal.local.pfx -nocerts -out key.pem
```

### Export the certificate file from the pfx file (cert.pem == identity-svc.crt)
```bash
openssl pkcs12 -in productgrpc.internal.local.pfx -clcerts -nokeys -out cert.pem
```

### Remove the passphrase from the private key (server.pem == identity-svc.pem)
```bash
openssl rsa -in key.pem -out server.pem
```


