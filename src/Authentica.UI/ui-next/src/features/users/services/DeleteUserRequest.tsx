import { BASE_URL } from "@/common/constants/BaseUrl";

export const DeleteUserRequest = async (): Promise<Response> => {
    const URL = `${BASE_URL}/users/delete`;
    const response = await fetch(URL, {
      method: 'DELETE',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };