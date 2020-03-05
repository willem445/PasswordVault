# Password Vault

Simple password manager using Argon2 and AES for password protection.

## Technology

### Cipher Suite

- Master password hashed with Argon2Id
  - Salt: 128 bit
  - Hash: 256 bit
  - Iterations: 20
  - Memory size: 1gb
  - Degree if parallelism: 8
    - ~30s execution time on Ryzen 2700X
- Sensitive user data and passwords encrypted using AES with Argon2Id key derivation and authenticated with HMAC256.
  - Argon2Id
    - Salt: 128 bit
    - Iterations: 40
    - Memory size: 1mb
    - Degree if parallelism: 4
      - ~120ms execution on Ryzen 2700X
  - AES
    - Key: 256 bit
    - Iv: 128 bit
  - HMAC
    - Key: 256 bit

### Storage

Data is stored in a local SQLite database using Dapper ORM for mapping.

## Screenshots

![Main](docs/resources/main.PNG)

## Build

TODO

## Unit Tests

TODO
