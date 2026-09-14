namespace Client.Common.Types

type UrlSegments = string list

type Session = {
    token: string
    username: string
}