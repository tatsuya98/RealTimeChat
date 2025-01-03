import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params}) => {   
    
        const response = await fetch(
        `https://localhost:7079/api/DirectMessage/${params.id}`
        );
        const data = await response.json();
        if(!response.ok){
            return {id: params.id, messages: []}
        }
        return {id: params.id, messages: data};
}