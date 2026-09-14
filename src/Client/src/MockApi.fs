namespace Client

// this is a mock api module, replace with real api calls
module MockApi =
    type AccessToken = AccessToken of string

    type User = {
        username: string
        token: AccessToken
    }

    type LoginRequest = {
        username: string
        password: string
    }

    type LoginResponse =
        | Authenticated of User
        | IncorrectCredentials

    let login (request: LoginRequest) = async {
        do! Async.Sleep 500 // simulate network delay

        if request.username = "admin" && request.password = "password" then
            return Authenticated {
                username = request.username
                token = AccessToken "mock-token-123"
            }
        else
            return IncorrectCredentials
    }

    let logout () = async {
        do! Async.Sleep 500 // simulate network delay
        return ()
    }