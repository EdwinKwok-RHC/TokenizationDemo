# TokenizationDemo
PCI DSS (Payment Card Industry Data Security Standard) Scope minimization via tokenization

## ScopeMinimizedTokenizationDemo
A basic .NET Core application for tokenization. 

### Test
1. Run the application and test the tokenization functionality.

**POST**
  ```sh
    http://localhost:5028/tokenize?contractAccount=123&contractNumber=123
   ```
Make note of the tokens such as

  ```sh
    {
        "contractAccountToken": "P1utVTju30-HPyTEub6v6w",
        "contractNumberToken": "XS0Vxu0XtEWvUzLIlb6Ntg"
    }
  ```
2. Verify that the tokenization service is working correctly.

**POST**
```sh
    http://localhost:5028/detokenize?token=P1utVTju30-HPyTEub6v6w
    http://localhost:5028/detokenize?token=XS0Vxu0XtEWvUzLIlb6Ntg
```

## TimedScopeMinimizedTokenizationDemo
Based on the ScopeMinimizedTokenizationDemo adding token expriation and clean up token function

### Test
1. Run the application and test the tokenization functionality.

**GET**
```sh
    https://localhost:7008/tokenizedJwt?accountNumber=123456&contractNumber=7890
```
Make note of the tokens such as
```sh
  {
    "jwtToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50TnVtYmVyIjoiNTY2ODdmYmEtMDBjMy00M2JkLThlOTktMjZmZmI4YTQ3YjgxIiwiY29udHJhY3ROdW1iZXIiOiIwM2NiMzg2Ny0zNzRmLTQ5NjAtYTczOS0wYWE2ZTcyNTUxYTgiLCJuYmYiOjE3NDY1NjI4MTEsImV4cCI6MTc0NjU2NjQxMSwiaWF0IjoxNzQ2NTYyODExfQ.olJFA56r1UqYv_aJH2NiT13ae-0QREY1mfGTMhATIKE"
  }
```
2. Verify that the tokenization service is working correctly.

**GET**
```sh
  https://localhost:7008/detokenizedJwt?jwtToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50TnVtYmVyIjoiNTY2ODdmYmEtMDBjMy00M2JkLThlOTktMjZmZmI4YTQ3YjgxIiwiY29udHJhY3ROdW1iZXIiOiIwM2NiMzg2Ny0zNzRmLTQ5NjAtYTczOS0wYWE2ZTcyNTUxYTgiLCJuYmYiOjE3NDY1NjI4MTEsImV4cCI6MTc0NjU2NjQxMSwiaWF0IjoxNzQ2NTYyODExfQ.olJFA56r1UqYv_aJH2NiT13ae-0QREY1mfGTMhATIKE
```
#### Response

```sh
{"accountNumber":"123456","contractNumber":"7890"}
```

#### raw JWT response

```sh
  {
    "accountNumber": "56687fba-00c3-43bd-8e99-26ffb8a47b81",
    "contractNumber": "03cb3867-374f-4960-a739-0aa6e72551a8",
    "nbf": 1746562811,
    "exp": 1746566411,
    "iat": 1746562811
  }
```


