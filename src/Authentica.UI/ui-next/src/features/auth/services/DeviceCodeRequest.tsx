import { BASE_URL } from "@/common/constants/BaseUrl";

export const DeviceCodeRequest = async (): Promise<Response> => {
    const URL = `${BASE_URL}/oauth2/device`;
    const response = await fetch(URL, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };