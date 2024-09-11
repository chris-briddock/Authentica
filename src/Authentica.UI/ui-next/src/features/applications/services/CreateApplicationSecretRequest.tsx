import { BASE_URL } from "@/common/constants/BaseUrl";


export const CreateApplicationSecretRequest = async (name: string): Promise<Response> => {
    const URL = `${BASE_URL}/applications/secrets`;
    const response = await fetch(URL, {
      method: 'PUT',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ name }),
    });
    return response;
  };