import { BASE_URL } from "@/common/constants/BaseUrl";
import { IRegisterRequest } from "../requests/IRegisterRequest";

export const RegisterRequest = async (data: IRegisterRequest): Promise<Response> => {
    const URL: string = `${BASE_URL}/users/register`
    
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
    
    