import { BASE_URL } from "@/common/constants/BaseUrl";
import { IAuthorizeRequest } from "@/features/applications/requests/IAuthorizeRequest";

export const AuthorizeRequest = async (data: IAuthorizeRequest): Promise<Response> => {
    const queryParams = new URLSearchParams({
      client_id: data.client_id,
      callback_uri: data.callback_uri,
      response_type: data.response_type,
      ...(data.code_challenge && { code_challenge: data.code_challenge }),
      ...(data.code_challenge_method && { code_challenge_method: data.code_challenge_method }),
    });
  
    const URL = `${BASE_URL}/oauth2/authorize?${queryParams.toString()}`;
    const response = await fetch(URL, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
      },
    });
    return response;
  };