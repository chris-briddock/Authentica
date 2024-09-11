import { BASE_URL } from "@/common/constants/BaseUrl";
import { ITokenRequest } from "../requests/ITokenRequest";

export const SendConfirmEmailTokenRequest = async (data: ITokenRequest): Promise<Response> => {
    const URL: string = `${BASE_URL}/users/tokens?email_address=${data.email}&token_type=${data.tokenType}`
    
    const response = await fetch(URL, {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });
    
    return response;
}
   