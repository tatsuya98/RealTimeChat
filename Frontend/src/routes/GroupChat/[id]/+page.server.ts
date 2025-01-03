import type { PageServerLoad } from "./$types";

export const load:PageServerLoad = async ({params}) => {
    const documentId = params.id;
    const response = await fetch(`https://localhost:7079/api/GroupChat/${documentId}`);
    const data = await response.json();
    return {id: documentId, messages: data};
}