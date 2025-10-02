class Utils {
    // Example: Format a date as YYYY-MM-DD
    static formatDate = (dateStr) => {
        if (!dateStr) return '';
        const date = new Date(dateStr);
        return date.toLocaleDateString(undefined, {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        });
    };

    static getImageUrl = (path) => {
        return path ? `http://localhost:5133/${path}`:'https://via.placeholder.com/300x400?text=No+Image';}

}

export default Utils;