import { BASE_URL } from "@/common/constants/BaseUrl";
import { IVerifyEmailRequest } from "@/features/auth/requests/IVerifyEmailRequest";

export const VerifyEmailRequest = async (data: IVerifyEmailRequest): Promise<Response> => {
  const URL: string = `${BASE_URL}/users/confirm-email?email=${data.email}&token=${data.token}`;
  
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