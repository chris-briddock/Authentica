import { BASE_URL } from "@/common/constants/BaseUrl";
import { ILoginData } from "@/features/auth/requests/ILoginRequest";

export const LoginRequest = async (data: ILoginData): Promise<Response> => {
    const URL: string = `${BASE_URL}/users/login`;
    
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