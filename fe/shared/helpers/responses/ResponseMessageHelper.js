const getResponseMessage = (jsonResponse, fallbackMessage) => {
    const message = jsonResponse?.message;

    return typeof message === "string" && message.trim()
        ? message
        : fallbackMessage;
};

const read = async (response, fallbackMessage) => {
    let jsonResponse = null;

    try {
        jsonResponse = await response.json();
    }
    catch {
        // Non-JSON and empty error responses should use the caller's fallback message.
    }

    return {
        jsonResponse,
        message: getResponseMessage(jsonResponse, fallbackMessage)
    };
};

export default {
    read
};
