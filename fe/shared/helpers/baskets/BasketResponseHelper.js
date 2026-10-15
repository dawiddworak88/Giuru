import ResponseMessageHelper from "../responses/ResponseMessageHelper";

const UnprocessableEntityStatus = 422;

// A save that finds the discount code stored on the basket no longer valid still saves the basket,
// repriced without the code, but answers with a 422 that carries the saved basket next to a message
// saying so. Every basket save reads its response through here: isSaved says whether the body is a
// saved basket to apply, and notice is the message to show with it when the basket was not saved
// exactly as it was sent. A failed save has no basket to apply - only its message.
const read = async (response, fallbackMessage) => {
    const { jsonResponse, message } = await ResponseMessageHelper.read(response, fallbackMessage);
    const isSaved = !!jsonResponse
        && (response.ok || (response.status === UnprocessableEntityStatus && !!jsonResponse.id));

    return {
        jsonResponse,
        message,
        isSaved,
        notice: isSaved && !response.ok ? message : null
    };
};

export default {
    read
};
