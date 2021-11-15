import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import Dialog from '@material-ui/core/Dialog';

export const useStyles = makeStyles(theme => ({
    root: {
        width: '100%',
        overflow: 'hidden',
        flex: 1
    },
    dialogPaper: {
        minHeight: 'calc(100vh - 30px)',
        maxHeight: 'calc(100vh - 30px)',
    },
}));

const IframeDialog = ({ src, children, ...props }) => {
    const classes = useStyles();

    return (
        <Dialog open fullWidth {...props} classes={{ paper: classes.dialogPaper }}>
            {children}
            <iframe id="iframe-dialog" src={src} frameBorder="0" className={classes.root} />
        </Dialog>
    )
}

export default IframeDialog