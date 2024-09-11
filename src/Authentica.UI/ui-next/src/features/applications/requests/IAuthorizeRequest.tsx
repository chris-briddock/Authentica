export interface IAuthorizeRequest {
    client_id: string;
    callback_uri: string;
    response_type: string;
    code_challenge?: string;
    code_challenge_method?: string;
}